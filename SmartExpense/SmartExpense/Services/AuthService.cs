using SmartExpense.Data;
using SmartExpense.Models;
using System.Security.Cryptography;
using System.Text;

namespace SmartExpense.Services;

public class AuthService(IUserRepository userRepository) : IAuthService
{
    private const string SessionKey = "smartexpense_session_username";

    public async Task<(bool success, string error)> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return (false, "Username and password are required.");

        var user = await userRepository.GetByUsernameAsync(username.Trim());
        if (user is null)
            return (false, "Invalid username or password.");

        var hash = HashPassword(password, username.Trim().ToLowerInvariant());
        if (!string.Equals(user.PasswordHash, hash, StringComparison.Ordinal))
            return (false, "Invalid username or password.");

        await SecureStorage.SetAsync(SessionKey, user.Username);
        return (true, string.Empty);
    }

    public async Task<(bool success, string error)> RegisterAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            return (false, "Username is required.");

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            return (false, "Password must be at least 6 characters.");

        var trimmed = username.Trim();

        if (await userRepository.UsernameExistsAsync(trimmed))
            return (false, "Username already taken. Please choose another.");

        var user = new AppUser
        {
            Username = trimmed,
            PasswordHash = HashPassword(password, trimmed.ToLowerInvariant()),
            CreatedAt = DateTime.UtcNow
        };

        await userRepository.CreateUserAsync(user);
        await SecureStorage.SetAsync(SessionKey, user.Username);
        return (true, string.Empty);
    }

    public async Task<bool> IsSessionActiveAsync()
    {
        var val = await SecureStorage.GetAsync(SessionKey);
        return !string.IsNullOrEmpty(val);
    }

    public Task<string?> GetSessionUsernameAsync()
        => SecureStorage.GetAsync(SessionKey)!;

    public Task LogoutAsync()
    {
        SecureStorage.Remove(SessionKey);
        return Task.CompletedTask;
    }

    // SHA-256 with username as salt
    private static string HashPassword(string password, string salt)
    {
        var input = $"{salt}:{password}";
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(bytes);
    }
}
