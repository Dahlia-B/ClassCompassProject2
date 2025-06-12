using ClassCompass.Shared.Models;

namespace ClassCompassApp.Services
{
    public class HomeworkApi
    {
        private readonly HttpClient _httpClient;
        
        public HomeworkApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<List<Assignment>> GetHomeworkAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/homework");
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Deserialize actual homework
                    return new List<Assignment>();
                }
                return new List<Assignment>();
            }
            catch
            {
                return new List<Assignment>();
            }
        }
        
        public async Task<bool> SubmitHomeworkAsync(Assignment assignment)
        {
            try
            {
                // TODO: Implement homework submission
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<Assignment> GetAssignmentAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/homework/{id}");
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Deserialize actual assignment
                    return new Assignment { Id = id, Title = "Sample Assignment" };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        
        public async Task<List<Assignment>> GetAssignmentsByStudentAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/homework/student/{studentId}");
                if (response.IsSuccessStatusCode)
                {
                    // TODO: Deserialize actual assignments
                    return new List<Assignment>();
                }
                return new List<Assignment>();
            }
            catch
            {
                return new List<Assignment>();
            }
        }
    }
}

