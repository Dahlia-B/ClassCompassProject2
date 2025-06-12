using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class GradesHttpService : IGradesHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/grades";

        public GradesHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Grade>> GetGradesByStudentAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/student/{studentId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Grade>>(json) ?? new List<Grade>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetGradesByStudentAsync: {ex.Message}");
                return new List<Grade>();
            }
        }

        public async Task<List<Grade>> GetGradesByAssignmentAsync(int assignmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/assignment/{assignmentId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Grade>>(json) ?? new List<Grade>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetGradesByAssignmentAsync: {ex.Message}");
                return new List<Grade>();
            }
        }

        public async Task<Grade> CreateGradeAsync(Grade grade)
        {
            var json = JsonConvert.SerializeObject(grade);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(BaseEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Grade>(responseJson)!;
        }

        public async Task<Grade> UpdateGradeAsync(Grade grade)
        {
            var json = JsonConvert.SerializeObject(grade);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/{grade.Id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Grade>(responseJson)!;
        }

        public async Task<bool> DeleteGradeAsync(int gradeId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/{gradeId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
