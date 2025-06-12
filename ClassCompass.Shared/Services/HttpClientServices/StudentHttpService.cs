using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class StudentHttpService : IStudentHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/students";

        public StudentHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseEndpoint);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAllStudentsAsync: {ex.Message}");
                return new List<Student>();
            }
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/{id}");
                if (!response.IsSuccessStatusCode) return null;
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Student>(json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetStudentByIdAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(BaseEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(responseJson)!;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/{student.Id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Student>(responseJson)!;
        }

        public async Task<bool> DeleteStudentAsync(int id)
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

        public async Task<List<Student>> GetStudentsByClassAsync(int classId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/class/{classId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Student>>(json) ?? new List<Student>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetStudentsByClassAsync: {ex.Message}");
                return new List<Student>();
            }
        }

        public async Task<bool> EnrollStudentInClassAsync(int studentId, int classId)
        {
            try
            {
                var enrollmentData = new { StudentId = studentId, ClassId = classId };
                var json = JsonConvert.SerializeObject(enrollmentData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseEndpoint}/{studentId}/enroll/{classId}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
