using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ClassCompass.Shared.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ClassCompass.Shared.Models;
using System.Text.Json;
using System.Text;

namespace ClassCompass.Shared.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl = "http://10.56.200.186:5004/";

        public ApiService()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_baseUrl); // ✅ FIXED: No more * symbols
            _client.Timeout = TimeSpan.FromSeconds(30);
        }

        // ✅ Test Connection - Working
        public async Task<string?> TestConnectionAsync()
        {
            try
            {
                var response = await _client.GetAsync("health");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                else
                {
                    return $"Health check failed: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        // ✅ School Registration - FIXED
        public async Task<SchoolResponse?> CreateSchoolAsync(SchoolRegistrationRequest school)
        {
            try
            {
                var json = JsonSerializer.Serialize(school);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/School", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<SchoolResponse>(jsonResult, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"School creation failed: {response.StatusCode} - {error}");
                    return new SchoolResponse { Success = false, Message = $"Failed: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateSchoolAsync: {ex.Message}");
                return new SchoolResponse { Success = false, Message = ex.Message };
            }
        }

        // ✅ Teacher Registration - NEW
        public async Task<TeacherResponse?> CreateTeacherAsync(TeacherRegistrationRequest teacher)
        {
            try
            {
                var json = JsonSerializer.Serialize(teacher);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/Teacher", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<TeacherResponse>(jsonResult, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Teacher creation failed: {response.StatusCode} - {error}");
                    return new TeacherResponse { Success = false, Message = $"Failed: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateTeacherAsync: {ex.Message}");
                return new TeacherResponse { Success = false, Message = ex.Message };
            }
        }

        // ✅ Student Registration - NEW  
        public async Task<StudentResponse?> CreateStudentAsync(StudentRegistrationRequest student)
        {
            try
            {
                var json = JsonSerializer.Serialize(student);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/Student", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<StudentResponse>(jsonResult, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Student creation failed: {response.StatusCode} - {error}");
                    return new StudentResponse { Success = false, Message = $"Failed: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in CreateStudentAsync: {ex.Message}");
                return new StudentResponse { Success = false, Message = ex.Message };
            }
        }

        // ✅ Login - NEW
        public async Task<AuthResponse?> LoginAsync(LoginRequest loginRequest)
        {
            try
            {
                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync("api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<AuthResponse>(jsonResult, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"Login failed: {response.StatusCode} - {error}");
                    return new AuthResponse { Success = false, Message = $"Login failed: {response.StatusCode}" };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoginAsync: {ex.Message}");
                return new AuthResponse { Success = false, Message = ex.Message };
            }
        }

        // ✅ Students - Working
        public async Task<List<Student>?> GetStudentsAsync()
        {
            try
            {
                return await _client.GetFromJsonAsync<List<Student>>("api/Student");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetStudents Error: {ex.Message}");
                return null;
            }
        }

        // ✅ Attendance Test - Working
        public async Task<object?> GetAttendanceTestAsync()
        {
            try
            {
                return await _client.GetFromJsonAsync<object>("api/Attendance/test");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAttendance Error: {ex.Message}");
                return null;
            }
        }

        // ✅ API Info - Working
        public async Task<string?> GetApiInfoAsync()
        {
            try
            {
                return await _client.GetStringAsync("info");
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }

    // ✅ Request Models
    public class SchoolRegistrationRequest
    {
        public string Name { get; set; } = "";
        public int? NumberOfClasses { get; set; }
        public string? Description { get; set; }
    }

    public class TeacherRegistrationRequest
    {
        public string Name { get; set; } = "";
        public string? Subject { get; set; }
        public int? SchoolId { get; set; }
        public string? PasswordHash { get; set; }
    }

    public class StudentRegistrationRequest
    {
        public string Name { get; set; } = "";
        public string? ClassName { get; set; }
        public int? TeacherId { get; set; }
        public int? ClassId { get; set; }
        public string? PasswordHash { get; set; }
    }

    // ✅ Response Models
    public class SchoolResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int SchoolId { get; set; }
        public object? Data { get; set; }
    }

    public class TeacherResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int TeacherId { get; set; }
        public object? Data { get; set; }
    }

    public class StudentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int StudentId { get; set; }
        public object? Data { get; set; }
    }

    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public string? Token { get; set; }
        public User? User { get; set; }
    }
}