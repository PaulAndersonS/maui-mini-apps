using SmartExpense.Models;

namespace SmartExpense.Services;

public interface IReportService
{
    Task<string> ExportMonthlyReportAsync(int year, int month, DashboardSummary summary, List<Transaction> transactions, List<CategoryBreakdownItem> categoryBreakdown);
}
