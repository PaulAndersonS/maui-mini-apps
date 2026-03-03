using SmartExpense.Models;

namespace SmartExpense.Data;

public class TransactionRepository(ExpenseDbContext dbContext) : ITransactionRepository
{
    private readonly ExpenseDbContext _dbContext = dbContext;

    public async Task<List<Transaction>> GetAllAsync()
    {
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.Table<Transaction>()
            .OrderByDescending(x => x.Date)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByMonthAsync(int year, int month)
    {
        var start = new DateTime(year, month, 1);
        var end = start.AddMonths(1);
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.Table<Transaction>()
            .Where(x => x.Date >= start && x.Date < end)
            .OrderByDescending(x => x.Date)
            .ToListAsync();
    }

    public async Task<List<Transaction>> GetByYearAsync(int year)
    {
        var start = new DateTime(year, 1, 1);
        var end = start.AddYears(1);
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.Table<Transaction>()
            .Where(x => x.Date >= start && x.Date < end)
            .OrderByDescending(x => x.Date)
            .ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(string id)
    {
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.Table<Transaction>()
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> InsertAsync(Transaction transaction)
    {
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.InsertAsync(transaction);
    }

    public async Task<int> UpdateAsync(Transaction transaction)
    {
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.UpdateAsync(transaction);
    }

    public async Task<int> DeleteAsync(Transaction transaction)
    {
        var connection = await _dbContext.GetConnectionAsync();
        return await connection.DeleteAsync(transaction);
    }
}
