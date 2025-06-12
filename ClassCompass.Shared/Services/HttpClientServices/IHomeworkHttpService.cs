using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface IHomeworkHttpService
    {
        Task<List<Assignment>> GetAssignmentsByClassAsync(int classId);
        Task<List<Assignment>> GetAssignmentsByStudentAsync(int studentId);
        Task<Assignment> CreateAssignmentAsync(Assignment assignment);
        Task<Assignment> UpdateAssignmentAsync(Assignment assignment);
        Task<bool> DeleteAssignmentAsync(int assignmentId);
        Task<List<HomeworkSubmission>> GetSubmissionsByAssignmentAsync(int assignmentId);
        Task<HomeworkSubmission> SubmitHomeworkAsync(HomeworkSubmission submission);
        Task<HomeworkSubmission> UpdateSubmissionAsync(HomeworkSubmission submission);
    }
}
