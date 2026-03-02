using SmartExpense.Data;
using SmartExpense.Models;

namespace SmartExpense.Services;

public class TransactionService(
    ITransactionRepository transactionRepository,
    ICategoryRepository categoryRepository) : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository = transactionRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public Task<List<Transaction>> GetAllTransactionsAsync() => _transactionRepository.GetAllAsync();

    public Task<List<Transaction>> GetTransactionsByMonthAsync(int year, int month) => _transactionRepository.GetByMonthAsync(year, month);

    public Task<List<Transaction>> GetTransactionsByYearAsync(int year) => _transactionRepository.GetByYearAsync(year);

    public Task<List<Category>> GetCategoriesAsync() => _categoryRepository.GetAllAsync();

    public async Task SaveTransactionAsync(Transaction transaction)
    {
        if (string.IsNullOrWhiteSpace(transaction.Id))
        {
            transaction.Id = Guid.NewGuid().ToString("N");
            await _transactionRepository.InsertAsync(transaction);
            return;
        }

        var existing = await _transactionRepository.GetByIdAsync(transaction.Id);
        if (existing is null)
        {
            await _transactionRepository.InsertAsync(transaction);
        }
        else
        {
            await _transactionRepository.UpdateAsync(transaction);
        }
    }

    public Task DeleteTransactionAsync(Transaction transaction) => _transactionRepository.DeleteAsync(transaction);

    public async Task<DashboardSummary> GetDashboardSummaryAsync()
    {
        var items = await _transactionRepository.GetAllAsync();
        var now = DateTime.Now;

        var totalIncome = items.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount);
        var totalExpense = items.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount);

        var currentMonth = items.Where(x => x.Date.Year == now.Year && x.Date.Month == now.Month).ToList();
        var currentMonthIncome = currentMonth.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount);
        var currentMonthExpense = currentMonth.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount);

        return new DashboardSummary
        {
            TotalIncome = totalIncome,
            TotalExpense = totalExpense,
            TotalBalance = totalIncome - totalExpense,
            CurrentMonthIncome = currentMonthIncome,
            CurrentMonthExpense = currentMonthExpense
        };
    }

    public async Task<List<MonthlyTrendPoint>> GetMonthlyTrendAsync(int year)
    {
        var items = await _transactionRepository.GetByYearAsync(year);
        var points = new List<MonthlyTrendPoint>();

        for (var month = 1; month <= 12; month++)
        {
            var monthItems = items.Where(x => x.Date.Month == month).ToList();
            points.Add(new MonthlyTrendPoint
            {
                Month = new DateTime(year, month, 1).ToString("MMM"),
                Income = monthItems.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount),
                Expense = monthItems.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount)
            });
        }

        return points;
    }

    public async Task<List<CategoryBreakdownItem>> GetCategoryBreakdownAsync(int year, int month)
    {
        var items = await _transactionRepository.GetByMonthAsync(year, month);
        var expenses = items.Where(x => x.Type == TransactionType.Expense).ToList();
        var total = expenses.Sum(x => x.Amount);

        if (total <= 0)
        {
            return [];
        }

        return expenses
            .GroupBy(x => x.Category)
            .Select(g => new CategoryBreakdownItem
            {
                Category = g.Key,
                Amount = g.Sum(x => x.Amount),
                Percentage = (double)(g.Sum(x => x.Amount) / total * 100)
            })
            .OrderByDescending(x => x.Amount)
            .ToList();
    }
}
