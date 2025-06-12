using System.Net.Http.Json;
using ClassCompass.Shared.Models;

namespace ClassCompassApp.Services
{
    public class AttendanceApi
    {
        private readonly HttpClient _httpClient;
        
        public AttendanceApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<Attendance>> GetAttendanceAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/attendance");
                if (response.IsSuccessStatusCode)
                {
                    var attendance = await response.Content.ReadFromJsonAsync<List<Attendance>>();
                    return attendance ?? new List<Attendance>();
                }
                return new List<Attendance>();
            }
            catch
            {
                return new List<Attendance>();
            }
        }
        
        public async Task<bool> MarkAttendanceAsync(Attendance attendance)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/attendance", attendance);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
