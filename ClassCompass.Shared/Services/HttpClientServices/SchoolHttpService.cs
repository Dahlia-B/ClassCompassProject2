using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class SchoolHttpService : ISchoolHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/school";  // Changed from api/schools

        public SchoolHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<School>> GetAllSchoolsAsync()
        {
            try
            {
                // Try different endpoint for getting all schools
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/all");
                if (!response.IsSuccessStatusCode)
                {
                    // Try another common pattern
                    response = await _httpClient.GetAsync(BaseEndpoint);
                }
                
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<School>>(json) ?? new List<School>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllSchoolsAsync: {ex.Message}");
                return new List<School>();
            }
        }

        public async Task<School?> GetSchoolByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/{id}");
                if (!response.IsSuccessStatusCode) return null;
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<School>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetSchoolByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<School> CreateSchoolAsync(School school)
        {
            try
            {
                var json = JsonConvert.SerializeObject(school);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(BaseEndpoint, content);
                response.EnsureSuccessStatusCode();
                
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<School>(responseJson)!;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateSchoolAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<School> UpdateSchoolAsync(School school)
        {
            var json = JsonConvert.SerializeObject(school);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/{school.SchoolId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<School>(responseJson)!;
        }

        public async Task<bool> DeleteSchoolAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
