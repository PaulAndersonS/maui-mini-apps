namespace SmartExpense.Services;

public interface IAuthService
{
    Task<(bool success, string error)> LoginAsync(string username, string password);
    Task<(bool success, string error)> RegisterAsync(string username, string password);
    Task<bool> IsSessionActiveAsync();
    Task<string?> GetSessionUsernameAsync();
    Task LogoutAsync();
}
