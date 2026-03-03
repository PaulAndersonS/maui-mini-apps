using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartExpense.Helpers;
using SmartExpense.Models;
using SmartExpense.Services;

namespace SmartExpense.ViewModels;

public partial class ReportsViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly IReportService _reportService;

    public ReportsViewModel(ITransactionService transactionService, IReportService reportService)
    {
        _transactionService = transactionService;
        _reportService = reportService;
        WeakReferenceMessenger.Default.Register<TransactionsChangedMessage>(this, async (_, _) =>
        {
            await LoadSummaryCommand.ExecuteAsync(null);
        });
    }

    [ObservableProperty]
    private DateTime selectedMonth = DateTime.Now;

    [ObservableProperty]
    private string monthSummary = string.Empty;

    [ObservableProperty]
    private string summaryIncomeText = "—";

    [ObservableProperty]
    private string summaryExpenseText = "—";

    [ObservableProperty]
    private string summaryNetText = "—";

    [ObservableProperty]
    private string? lastReportPath;

    [RelayCommand]
    private async Task LoadSummaryAsync()
    {
        var items = await _transactionService.GetTransactionsByMonthAsync(SelectedMonth.Year, SelectedMonth.Month);
        var income = items.Where(x => x.Type == TransactionType.Income).Sum(x => x.Amount);
        var expense = items.Where(x => x.Type == TransactionType.Expense).Sum(x => x.Amount);
        var net = income - expense;
        SummaryIncomeText = income.ToString(AppConstants.CurrencyFormat);
        SummaryExpenseText = expense.ToString(AppConstants.CurrencyFormat);
        SummaryNetText = net.ToString(AppConstants.CurrencyFormat);
        MonthSummary = $"Income: {income:C2} | Expense: {expense:C2} | Net: {net:C2}";
    }

    [RelayCommand]
    private async Task ExportPdfAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        try
        {
            var summary = await _transactionService.GetDashboardSummaryAsync();
            var items = await _transactionService.GetTransactionsByMonthAsync(SelectedMonth.Year, SelectedMonth.Month);
            var breakdown = await _transactionService.GetCategoryBreakdownAsync(SelectedMonth.Year, SelectedMonth.Month);

            LastReportPath = await _reportService.ExportMonthlyReportAsync(SelectedMonth.Year, SelectedMonth.Month, summary, items, breakdown);
            await Share.RequestAsync(new ShareFileRequest
            {
                Title = "Share Monthly Report",
                File = new ShareFile(LastReportPath)
            });
        }
        finally
        {
            IsBusy = false;
        }
    }
}
