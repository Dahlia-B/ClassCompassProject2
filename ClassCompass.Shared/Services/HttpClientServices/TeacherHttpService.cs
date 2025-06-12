using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class TeacherHttpService : ITeacherHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/teachers";

        public TeacherHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseEndpoint);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Teacher>>(json) ?? new List<Teacher>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllTeachersAsync: {ex.Message}");
                return new List<Teacher>();
            }
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/{id}");
                if (!response.IsSuccessStatusCode) return null;
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Teacher>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTeacherByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            var json = JsonConvert.SerializeObject(teacher);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(BaseEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Teacher>(responseJson)!;
        }

        public async Task<Teacher> UpdateTeacherAsync(Teacher teacher)
        {
            var json = JsonConvert.SerializeObject(teacher);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/{teacher.Id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Teacher>(responseJson)!;
        }

        public async Task<bool> DeleteTeacherAsync(int id)
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

        public async Task<List<Class>> GetTeacherClassesAsync(int teacherId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/{teacherId}/classes");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Class>>(json) ?? new List<Class>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTeacherClassesAsync: {ex.Message}");
                return new List<Class>();
            }
        }
    }
}
