using SmartExpense.Models;

namespace SmartExpense.Data;

public class UserRepository(ExpenseDbContext context) : IUserRepository
{
    public async Task<AppUser?> GetByUsernameAsync(string username)
    {
        var db = await context.GetConnectionAsync();
        return await db.Table<AppUser>()
                       .Where(u => u.Username == username)
                       .FirstOrDefaultAsync();
    }

    public async Task<int> CreateUserAsync(AppUser user)
    {
        var db = await context.GetConnectionAsync();
        return await db.InsertAsync(user);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        var db = await context.GetConnectionAsync();
        return await db.Table<AppUser>()
                       .Where(u => u.Username == username)
                       .CountAsync() > 0;
    }
}
