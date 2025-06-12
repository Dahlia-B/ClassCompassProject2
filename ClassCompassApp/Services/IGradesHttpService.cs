using System.Diagnostics;
using ClassCompass.Shared.Models;

namespace ClassCompassApp.Services
{
    public class IGradesHttpService
    {
        private readonly HttpClient _httpClient;
        
        public IGradesHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<Grade>> GetGradesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/grades");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    // TODO: Deserialize and return actual grades
                    return new List<Grade>();
                }
                return new List<Grade>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting grades: {ex.Message}");
                return new List<Grade>();
            }
        }
        
        public async Task<bool> SubmitGradeAsync(Grade grade)
        {
            try
            {
                // TODO: Implement grade submission
                await Task.Delay(100); // Simulate network call
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error submitting grade: {ex.Message}");
                return false;
            }
        }
        
        public async Task<List<Assignment>> GetAssignmentsAsync(int classId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/assignments/{classId}");
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Deserialize and return actual assignments
                    return new List<Assignment>();
                }
                return new List<Assignment>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting assignments: {ex.Message}");
                return new List<Assignment>();
            }
        }
    }
}
