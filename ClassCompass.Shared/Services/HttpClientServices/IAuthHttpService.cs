namespace ClassCompass.Shared.Services.HttpClientServices
{
    public interface IAuthHttpService
    {
        Task<string?> LoginAsync(string username, string password);
        Task<bool> RegisterAsync(string username, string password, string email, string role = "Student");
        Task<bool> LogoutAsync();
        Task<bool> ValidateTokenAsync(string token);
        Task<string?> RefreshTokenAsync(string refreshToken);
    }
}
