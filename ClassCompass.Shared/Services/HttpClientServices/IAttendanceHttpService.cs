using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface IAttendanceHttpService
    {
        Task<List<Attendance>> GetAttendanceByStudentAsync(int studentId, DateTime? date = null);
        Task<List<Attendance>> GetAttendanceByClassAsync(int classId, DateTime date);
        Task<Attendance> MarkAttendanceAsync(Attendance attendance);
        Task<bool> UpdateAttendanceAsync(Attendance attendance);
    }
}
