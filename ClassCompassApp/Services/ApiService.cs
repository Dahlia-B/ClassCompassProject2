using System.Net.Http.Json;
using ClassCompass.Shared.Models;

namespace ClassCompassApp.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        
        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { Username = username, Password = password };
                var response = await _httpClient.PostAsJsonAsync("/api/auth/login", loginData);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<bool> RegisterUserAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/users/register", user);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
        
        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/users");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<User>>();
                    return users ?? new List<User>();
                }
                return new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }
    }
}
