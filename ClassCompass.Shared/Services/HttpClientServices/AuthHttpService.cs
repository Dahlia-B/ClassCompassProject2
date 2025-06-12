using Newtonsoft.Json;
using System.Text;

namespace ClassCompass.Shared.Services.HttpClientServices
{
    public class AuthHttpService : IAuthHttpService
    {
        private readonly HttpClient _httpClient;
        private const string BaseEndpoint = "api/auth";

        public AuthHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { Username = username, Password = password };
                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseEndpoint}/login", content);
                if (!response.IsSuccessStatusCode) return null;

                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result?.token;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoginAsync: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(string username, string password, string email, string role = "Student")
        {
            try
            {
                var registerData = new 
                { 
                    Username = username, 
                    Password = password, 
                    Email = email,
                    Role = role
                };
                var json = JsonConvert.SerializeObject(registerData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseEndpoint}/register", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in RegisterAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync($"{BaseEndpoint}/logout", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                
                var response = await _httpClient.GetAsync($"{BaseEndpoint}/validate");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string?> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var refreshData = new { RefreshToken = refreshToken };
                var json = JsonConvert.SerializeObject(refreshData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{BaseEndpoint}/refresh", content);
                if (!response.IsSuccessStatusCode) return null;

                var responseJson = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<dynamic>(responseJson);
                return result?.token;
            }
            catch
            {
                return null;
            }
        }
    }
}
