using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface IStudentHttpService
    {
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student?> GetStudentByIdAsync(int id);
        Task<Student> CreateStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<List<Student>> GetStudentsByClassAsync(int classId);
        Task<bool> EnrollStudentInClassAsync(int studentId, int classId);
    }
}
