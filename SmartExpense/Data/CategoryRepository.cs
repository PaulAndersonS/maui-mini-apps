using SmartExpense.Models;
using SmartExpense.Helpers;
using SQLite;

namespace SmartExpense.Data;

public class CategoryRepository(ExpenseDbContext dbContext) : ICategoryRepository
{
    private readonly ExpenseDbContext _dbContext = dbContext;

    public async Task<List<Category>> GetAllAsync()
    {
        var connection = await _dbContext.GetConnectionAsync();
        try
        {
            return await connection.Table<Category>()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
        catch (SQLiteException exception) when (exception.Message.Contains("no such table", StringComparison.OrdinalIgnoreCase))
        {
            await connection.CreateTableAsync<Category>();
            await connection.InsertAllAsync(CategorySeedData.GetDefaultCategories());

            return await connection.Table<Category>()
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}
