using SQLite;
using SmartExpense.Helpers;
using SmartExpense.Models;

namespace SmartExpense.Data;

public class ExpenseDbContext
{
    private SQLiteAsyncConnection? _connection;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection is not null)
        {
            return _connection;
        }

        await _semaphore.WaitAsync();
        try
        {
            if (_connection is null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, AppConstants.DatabaseName);
                _connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
                await InitializeAsync(_connection);
            }
        }
        finally
        {
            _semaphore.Release();
        }

        return _connection;
    }

    private static async Task InitializeAsync(SQLiteAsyncConnection connection)
    {
        await connection.CreateTableAsync<Transaction>();
        await connection.CreateTableAsync<Category>();
        await connection.CreateTableAsync<AppUser>();

        var categoryCount = await connection.Table<Category>().CountAsync();
        if (categoryCount == 0)
        {
            await connection.InsertAllAsync(CategorySeedData.GetDefaultCategories());
        }
    }
}
