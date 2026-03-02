using SmartExpense.Models;

namespace SmartExpense.Services;

public interface ITransactionService
{
    Task<List<Transaction>> GetAllTransactionsAsync();
    Task<List<Transaction>> GetTransactionsByMonthAsync(int year, int month);
    Task<List<Transaction>> GetTransactionsByYearAsync(int year);
    Task<List<Category>> GetCategoriesAsync();
    Task SaveTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(Transaction transaction);
    Task<DashboardSummary> GetDashboardSummaryAsync();
    Task<List<MonthlyTrendPoint>> GetMonthlyTrendAsync(int year);
    Task<List<CategoryBreakdownItem>> GetCategoryBreakdownAsync(int year, int month);
}
