using ClassCompass.Shared.Interfaces;
using ClassCompass.Shared.Services.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
namespace ClassCompass.Shared.Services.Http
{
    public interface IGradesHttpService
    {
        Task<List<Grade>?> GetGradesAsync(string studentId, CancellationToken cancellationToken = default);
        Task<Grade?> GetGradeAsync(string studentId, string gradeId, CancellationToken cancellationToken = default);
        Task<bool> UpdateGradeAsync(string studentId, Grade grade, CancellationToken cancellationToken = default);
        Task<bool> DeleteGradeAsync(string studentId, string gradeId, CancellationToken cancellationToken = default);
        Task<List<Subject>?> GetSubjectsAsync(CancellationToken cancellationToken = default);
    }

    public class GradesHttpService : BaseHttpService, IGradesHttpService
    {
        private const string BaseEndpoint = "api/grades";

        public GradesHttpService(HttpClient httpClient, ILogger logger)
    : base(httpClient, logger)
        {
        }

        public async Task<List<Grade>?> GetGradesAsync(string studentId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetAsync<List<Grade>>($"{BaseEndpoint}/student/{studentId}", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting grades: {ex.Message}");
                return null;
            }
        }

        public async Task<Grade?> GetGradeAsync(string studentId, string gradeId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetAsync<Grade>($"{BaseEndpoint}/student/{studentId}/grade/{gradeId}", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting grade: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateGradeAsync(string studentId, Grade grade, CancellationToken cancellationToken = default)
        {
            try
            {
                return await PostAsync($"{BaseEndpoint}/student/{studentId}/grade/{grade.Id}", grade, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating grade: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteGradeAsync(string studentId, string gradeId, CancellationToken cancellationToken = default)
        {
            try
            {
                return await DeleteAsync($"{BaseEndpoint}/student/{studentId}/grade/{gradeId}", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting grade: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Subject>?> GetSubjectsAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await GetAsync<List<Subject>>($"{BaseEndpoint}/subjects", cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting subjects: {ex.Message}");
                return null;
            }
        }
    }

    // Grade model
    public class Grade
    {
        public string Id { get; set; } = string.Empty;
        public string StudentId { get; set; } = string.Empty;
        public string SubjectId { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public decimal Value { get; set; }
        public string Type { get; set; } = string.Empty; // Test, Quiz, Assignment, etc.
        public DateTime DateRecorded { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; } = 1.0m;
    }

    // Subject model
    public class Subject
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string TeacherId { get; set; } = string.Empty;
        public string TeacherName { get; set; } = string.Empty;
    }
}
