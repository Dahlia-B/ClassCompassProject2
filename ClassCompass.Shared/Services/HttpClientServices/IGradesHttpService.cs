using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface IGradesHttpService
    {
        Task<List<Grade>> GetGradesByStudentAsync(int studentId);
        Task<List<Grade>> GetGradesByAssignmentAsync(int assignmentId);
        Task<Grade> CreateGradeAsync(Grade grade);
        Task<Grade> UpdateGradeAsync(Grade grade);
        Task<bool> DeleteGradeAsync(int gradeId);
    }
}
