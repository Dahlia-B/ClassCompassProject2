using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class AttendanceHttpService : IAttendanceHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/attendance";

        public AttendanceHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Attendance>> GetAttendanceByStudentAsync(int studentId, DateTime? date = null)
        {
            try
            {
                var endpoint = $"{BaseEndpoint}/student/{studentId}";
                if (date.HasValue)
                    endpoint += $"?date={date.Value:yyyy-MM-dd}";

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Attendance>>(json) ?? new List<Attendance>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAttendanceByStudentAsync: {ex.Message}");
                return new List<Attendance>();
            }
        }

        public async Task<List<Attendance>> GetAttendanceByClassAsync(int classId, DateTime date)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/class/{classId}?date={date:yyyy-MM-dd}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Attendance>>(json) ?? new List<Attendance>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAttendanceByClassAsync: {ex.Message}");
                return new List<Attendance>();
            }
        }

        public async Task<Attendance> MarkAttendanceAsync(Attendance attendance)
        {
            var json = JsonConvert.SerializeObject(attendance);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(BaseEndpoint, content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Attendance>(responseJson)!;
        }

        public async Task<bool> UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                var json = JsonConvert.SerializeObject(attendance);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                // Use AttendanceId (Guid) as the correct property name
                var response = await _httpClient.PutAsync($"{BaseEndpoint}/{attendance.AttendanceId}", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
