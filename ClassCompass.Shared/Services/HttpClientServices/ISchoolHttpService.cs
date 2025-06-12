using ClassCompass.Shared.Models;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface ISchoolHttpService
    {
        Task<List<School>> GetAllSchoolsAsync();
        Task<School?> GetSchoolByIdAsync(int id);
        Task<School> CreateSchoolAsync(School school);
        Task<School> UpdateSchoolAsync(School school);
        Task<bool> DeleteSchoolAsync(int id);
    }
}
