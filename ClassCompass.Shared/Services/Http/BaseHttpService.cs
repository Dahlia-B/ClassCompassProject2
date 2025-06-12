using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ClassCompass.Shared.Services.Http
{
    public abstract class BaseHttpService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILogger _logger;
        protected readonly JsonSerializerOptions _jsonOptions;

        protected BaseHttpService(HttpClient httpClient, ILogger logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        protected async Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making GET request to: {Endpoint}", endpoint);

                var response = await _httpClient.GetAsync(endpoint, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions);
                }

                _logger.LogWarning("GET request failed. Status: {StatusCode}", response.StatusCode);
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during GET request to {Endpoint}", endpoint);
                return default(T);
            }
        }

        protected async Task<T?> PostAsync<T>(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making POST request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }

                _logger.LogWarning("POST request failed. Status: {StatusCode}", response.StatusCode);
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during POST request to {Endpoint}", endpoint);
                return default(T);
            }
        }

        protected async Task<bool> PostAsync(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making POST request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during POST request to {Endpoint}", endpoint);
                return false;
            }
        }

        protected async Task<T?> PutAsync<T>(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making PUT request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }

                _logger.LogWarning("PUT request failed. Status: {StatusCode}", response.StatusCode);
                return default(T);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during PUT request to {Endpoint}", endpoint);
                return default(T);
            }
        }

        protected async Task<bool> PutAsync(string endpoint, object data, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making PUT request to: {Endpoint}", endpoint);

                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content, cancellationToken);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during PUT request to {Endpoint}", endpoint);
                return false;
            }
        }

        protected async Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Making DELETE request to: {Endpoint}", endpoint);

                var response = await _httpClient.DeleteAsync(endpoint, cancellationToken);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during DELETE request to {Endpoint}", endpoint);
                return false;
            }
        }
    }
}