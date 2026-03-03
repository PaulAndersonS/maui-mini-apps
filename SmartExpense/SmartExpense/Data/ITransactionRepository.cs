using SmartExpense.Models;

namespace SmartExpense.Data;

public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync();
    Task<List<Transaction>> GetByMonthAsync(int year, int month);
    Task<List<Transaction>> GetByYearAsync(int year);
    Task<Transaction?> GetByIdAsync(string id);
    Task<int> InsertAsync(Transaction transaction);
    Task<int> UpdateAsync(Transaction transaction);
    Task<int> DeleteAsync(Transaction transaction);
}
