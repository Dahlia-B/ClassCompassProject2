using ClassCompass.Shared.Models;
using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class HomeworkHttpService : IHomeworkHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/homework";

        public HomeworkHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Assignment>> GetAssignmentsByClassAsync(int classId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/class/{classId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Assignment>>(json) ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAssignmentsByClassAsync: {ex.Message}");
                return new List<Assignment>();
            }
        }

        public async Task<List<Assignment>> GetAssignmentsByStudentAsync(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/student/{studentId}");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Assignment>>(json) ?? new List<Assignment>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetAssignmentsByStudentAsync: {ex.Message}");
                return new List<Assignment>();
            }
        }

        public async Task<Assignment> CreateAssignmentAsync(Assignment assignment)
        {
            var json = JsonConvert.SerializeObject(assignment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{BaseEndpoint}/assignments", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Assignment>(responseJson)!;
        }

        public async Task<Assignment> UpdateAssignmentAsync(Assignment assignment)
        {
            var json = JsonConvert.SerializeObject(assignment);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/assignments/{assignment.Id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Assignment>(responseJson)!;
        }

        public async Task<bool> DeleteAssignmentAsync(int assignmentId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{BaseEndpoint}/assignments/{assignmentId}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<HomeworkSubmission>> GetSubmissionsByAssignmentAsync(int assignmentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/assignments/{assignmentId}/submissions");
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HomeworkSubmission>>(json) ?? new List<HomeworkSubmission>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetSubmissionsByAssignmentAsync: {ex.Message}");
                return new List<HomeworkSubmission>();
            }
        }

        public async Task<HomeworkSubmission> SubmitHomeworkAsync(HomeworkSubmission submission)
        {
            var json = JsonConvert.SerializeObject(submission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"{BaseEndpoint}/submissions", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HomeworkSubmission>(responseJson)!;
        }

        public async Task<HomeworkSubmission> UpdateSubmissionAsync(HomeworkSubmission submission)
        {
            var json = JsonConvert.SerializeObject(submission);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            // Use SubmissionId (int) as the correct property name
            var response = await _httpClient.PutAsync($"{BaseEndpoint}/submissions/{submission.SubmissionId}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HomeworkSubmission>(responseJson)!;
        }
    }
}
