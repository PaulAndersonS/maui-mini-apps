using SmartExpense.Models;

namespace SmartExpense.Data;

public interface IUserRepository
{
    Task<AppUser?> GetByUsernameAsync(string username);
    Task<int> CreateUserAsync(AppUser user);
    Task<bool> UsernameExistsAsync(string username);
}
