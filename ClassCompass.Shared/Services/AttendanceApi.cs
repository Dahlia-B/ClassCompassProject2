using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ClassCompass.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Dapper;

namespace ClassCompass.Shared.Services
{
    public class AttendanceApi
    {
        private readonly string _connectionString;
        private readonly ILogger<AttendanceApi> _logger;

        public AttendanceApi(IConfiguration configuration, ILogger<AttendanceApi> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                               ?? throw new InvalidOperationException("DefaultConnection string is missing");
            _logger = logger;
        }

        public async Task<bool> MarkAttendance(string studentId, Attendance record)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Parse studentId to int to match your model
                if (!int.TryParse(studentId, out int studentIdInt))
                {
                    _logger.LogError("Invalid student ID format: {StudentId}", studentId);
                    return false;
                }

                // Check if attendance already exists for this student on this date in this classroom
                var existingRecord = await connection.QueryFirstOrDefaultAsync<Attendance>(
                    "SELECT * FROM Attendance WHERE StudentId = @StudentId AND DATE(Date) = DATE(@Date) AND ClassRoomId = @ClassRoomId",
                    new { StudentId = studentIdInt, Date = record.Date, ClassRoomId = record.ClassRoomId });

                if (existingRecord != null)
                {
                    // Update existing record
                    var updateSql = @"
                        UPDATE Attendance 
                        SET Present = @Present, 
                            Notes = @Notes, 
                            Date = @Date 
                        WHERE AttendanceId = @AttendanceId";

                    var rowsAffected = await connection.ExecuteAsync(updateSql, new
                    {
                        Present = record.Present,
                        Notes = record.Notes,
                        Date = DateTime.Now,
                        AttendanceId = existingRecord.AttendanceId
                    });

                    _logger.LogInformation("Updated attendance record for student {StudentId} on {Date}",
                        studentId, record.Date.Date);

                    return rowsAffected > 0;
                }
                else
                {
                    // Insert new record
                    var insertSql = @"
                        INSERT INTO Attendance (AttendanceId, StudentId, Present, Date, Notes, ClassRoomId)
                        VALUES (@AttendanceId, @StudentId, @Present, @Date, @Notes, @ClassRoomId)";

                    var rowsAffected = await connection.ExecuteAsync(insertSql, new
                    {
                        AttendanceId = Guid.NewGuid(),
                        StudentId = studentIdInt,
                        Present = record.Present,
                        Date = record.Date,
                        Notes = record.Notes,
                        ClassRoomId = record.ClassRoomId
                    });

                    _logger.LogInformation("Created new attendance record for student {StudentId} on {Date}",
                        studentId, record.Date.Date);

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking attendance for student {StudentId}", studentId);
                return false;
            }
        }

        public async Task<List<Attendance>> GetAttendanceRecords(string studentId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Parse studentId to int to match your model
                if (!int.TryParse(studentId, out int studentIdInt))
                {
                    _logger.LogError("Invalid student ID format: {StudentId}", studentId);
                    return new List<Attendance>();
                }

                var sql = @"
                    SELECT AttendanceId, StudentId, Present, Date, Notes, ClassRoomId
                    FROM Attendance 
                    WHERE StudentId = @StudentId 
                    ORDER BY Date DESC";

                var records = await connection.QueryAsync<Attendance>(sql, new { StudentId = studentIdInt });

                _logger.LogInformation("Retrieved {Count} attendance records for student {StudentId}",
                    records.Count(), studentId);

                return records.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving attendance records for student {StudentId}", studentId);
                return new List<Attendance>();
            }
        }

        public async Task<bool> UpdateAttendance(Attendance record)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = @"
                    UPDATE Attendance 
                    SET Present = @Present, 
                        Notes = @Notes, 
                        Date = @Date,
                        ClassRoomId = @ClassRoomId
                    WHERE AttendanceId = @AttendanceId";

                var rowsAffected = await connection.ExecuteAsync(sql, new
                {
                    Present = record.Present,
                    Notes = record.Notes,
                    Date = record.Date,
                    ClassRoomId = record.ClassRoomId,
                    AttendanceId = record.AttendanceId
                });

                _logger.LogInformation("Updated attendance record {AttendanceId}", record.AttendanceId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating attendance record {AttendanceId}", record.AttendanceId);
                return false;
            }
        }

        public async Task<List<Attendance>> GetClassAttendanceByDate(string classId, DateTime date)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Parse classId to int
                if (!int.TryParse(classId, out int classIdInt))
                {
                    _logger.LogError("Invalid class ID format: {ClassId}", classId);
                    return new List<Attendance>();
                }

                var sql = @"
                    SELECT AttendanceId, StudentId, Present, Date, Notes, ClassRoomId
                    FROM Attendance
                    WHERE ClassRoomId = @ClassRoomId 
                    AND DATE(Date) = DATE(@Date)
                    ORDER BY StudentId";

                var records = await connection.QueryAsync<Attendance>(sql, new
                {
                    ClassRoomId = classIdInt,
                    Date = date
                });

                _logger.LogInformation("Retrieved {Count} attendance records for class {ClassId} on {Date}",
                    records.Count(), classId, date.Date);

                return records.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving class attendance for class {ClassId} on {Date}", classId, date);
                return new List<Attendance>();
            }
        }

        public async Task<AttendanceSummary> GetAttendanceSummary(string studentId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                // Parse studentId to int
                if (!int.TryParse(studentId, out int studentIdInt))
                {
                    _logger.LogError("Invalid student ID format: {StudentId}", studentId);
                    return new AttendanceSummary { StudentId = studentId };
                }

                var whereClause = "WHERE StudentId = @StudentId";
                object parameters = new { StudentId = studentIdInt };

                if (startDate.HasValue && endDate.HasValue)
                {
                    whereClause += " AND Date BETWEEN @StartDate AND @EndDate";
                    parameters = new { StudentId = studentIdInt, StartDate = startDate.Value, EndDate = endDate.Value };
                }

                var sql = $@"
                    SELECT 
                        COUNT(*) as TotalDays,
                        SUM(CASE WHEN Present = 1 THEN 1 ELSE 0 END) as PresentDays,
                        SUM(CASE WHEN Present = 0 THEN 1 ELSE 0 END) as AbsentDays
                    FROM Attendance 
                    {whereClause}";

                var summary = await connection.QueryFirstOrDefaultAsync(sql, parameters);

                var attendanceSummary = new AttendanceSummary
                {
                    StudentId = studentId,
                    TotalDays = summary?.TotalDays ?? 0,
                    PresentDays = summary?.PresentDays ?? 0,
                    AbsentDays = summary?.AbsentDays ?? 0,
                    AttendanceRate = summary?.TotalDays > 0 ?
                        Math.Round((double)(summary.PresentDays) / summary.TotalDays * 100, 2) : 0
                };

                _logger.LogInformation("Calculated attendance summary for student {StudentId}: {Rate}% attendance",
                    studentId, attendanceSummary.AttendanceRate);

                return attendanceSummary;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating attendance summary for student {StudentId}", studentId);
                return new AttendanceSummary { StudentId = studentId };
            }
        }

        public async Task<bool> DeleteAttendanceRecord(Guid attendanceId)
        {
            try
            {
                using var connection = new MySqlConnection(_connectionString);
                await connection.OpenAsync();

                var sql = "DELETE FROM Attendance WHERE AttendanceId = @AttendanceId";
                var rowsAffected = await connection.ExecuteAsync(sql, new { AttendanceId = attendanceId });

                _logger.LogInformation("Deleted attendance record {AttendanceId}", attendanceId);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting attendance record {AttendanceId}", attendanceId);
                return false;
            }
        }
    }

    // Helper class for attendance summary
    public class AttendanceSummary
    {
        public required string StudentId { get; set; }
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public double AttendanceRate { get; set; }
    }
}