using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SmartExpense.Helpers;
using SmartExpense.Models;
using SmartExpense.Services;
using System.Collections.ObjectModel;
using System.Text;

namespace SmartExpense.ViewModels;

public partial class DashboardViewModel : BaseViewModel
{
    private readonly ITransactionService _transactionService;
    private readonly IAuthService _authService;

    public DashboardViewModel(ITransactionService transactionService, IAuthService authService)
    {
        _transactionService = transactionService;
        _authService = authService;
        WeakReferenceMessenger.Default.Register<TransactionsChangedMessage>(this, async (_, _) =>
        {
            await LoadCommand.ExecuteAsync(null);
        });
    }

    public ObservableCollection<MonthlyTrendPoint> MonthlyTrends { get; } = [];
    public ObservableCollection<CategoryBreakdownItem> CategoryBreakdown { get; } = [];
    public ObservableCollection<string> MonthOptions { get; } =
    [
        "January", "February", "March", "April", "May", "June",
        "July", "August", "September", "October", "November", "December"
    ];

    [ObservableProperty]
    private decimal totalBalance;

    [ObservableProperty]
    private decimal totalIncome;

    [ObservableProperty]
    private decimal totalExpense;

    [ObservableProperty]
    private decimal currentMonthIncome;

    [ObservableProperty]
    private decimal currentMonthExpense;

    [ObservableProperty]
    private string selectedYear = DateTime.Now.Year.ToString();

    [ObservableProperty]
    private bool isLoading;

    [ObservableProperty]
    private string totalBalanceText = string.Empty;

    [ObservableProperty]
    private string totalIncomeText = string.Empty;

    [ObservableProperty]
    private string totalExpenseText = string.Empty;

    [ObservableProperty]
    private string savingsText = string.Empty;

    [ObservableProperty]
    private string savingsPercentText = "0%";

    [ObservableProperty]
    private string userName = "User";

    partial void OnUserNameChanged(string value) => OnPropertyChanged(nameof(GreetingText));

    public string GreetingText
    {
        get
        {
            var hour = DateTime.Now.Hour;
            var salutation = hour switch
            {
                < 12 => "Good Morning",
                < 17 => "Good Afternoon",
                _    => "Good Evening"
            };
            return $"{salutation}, {UserName} 👋";
        }
    }

    [ObservableProperty]
    private string selectedMonth = DateTime.Now.ToString("MMMM");

    [ObservableProperty]
    private Color balanceColor = Colors.White;

    partial void OnSelectedMonthChanged(string value)
    {
        // Re-load when the user picks a different month in the header combo
        if (!IsBusy)
            LoadCommand.ExecuteAsync(null);
    }

    [RelayCommand]
    private void NavigateToAdd()
        => WeakReferenceMessenger.Default.Send(new NavigateToTabMessage(1));

    [RelayCommand]
    private void NavigateToReports()
        => WeakReferenceMessenger.Default.Send(new NavigateToTabMessage(3));

    [RelayCommand]
    private async Task LogoutAsync()
    {
        await _authService.LogoutAsync();
        var app = Application.Current;
        if (app is null) return;
        // Navigate back to login
        var services = IPlatformApplication.Current?.Services;
        if (services is null) return;
        var loginPage = services.GetRequiredService<SmartExpense.Views.LoginPage>();
        app.Windows[0].Page = new NavigationPage(loginPage);
    }

    [RelayCommand]
    private async Task ShowAISummaryAsync()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Balance:   {TotalBalanceText}");
        sb.AppendLine($"Income:    {TotalIncomeText}");
        sb.AppendLine($"Expenses:  {TotalExpenseText}");
        sb.AppendLine($"Savings:   {SavingsText} ({SavingsPercentText})");
        sb.AppendLine();

        // Simple rule-based insights
        if (TotalIncome == 0)
        {
            sb.AppendLine("📌 No income recorded yet. Add transactions to see insights.");
        }
        else
        {
            var savingsRate = TotalIncome > 0 ? (TotalIncome - TotalExpense) / TotalIncome * 100 : 0;
            if (savingsRate >= 20)
                sb.AppendLine($"✅ Great job! You are saving {savingsRate:F0}% of your income.");
            else if (savingsRate > 0)
                sb.AppendLine($"⚠️ Your savings rate is {savingsRate:F0}%. Aim for 20%+ for financial health.");
            else
                sb.AppendLine("🔴 Expenses exceed income this period. Consider reviewing your spending.");

            if (CategoryBreakdown.Count > 0)
            {
                var top = CategoryBreakdown.OrderByDescending(c => c.Percentage).First();
                sb.AppendLine($"📊 Top spending category: {top.Category} ({top.Percentage:F1}%)");
            }
        }

        var page = Application.Current?.Windows[0].Page;
        if (page is not null)
            await page.DisplayAlertAsync("💡 AI Summary", sb.ToString(), "OK");
    }

    [RelayCommand]
    private async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        IsBusy = true;
        IsLoading = true;
        try
        {
            // Load real username from session
            var sessionUser = await _authService.GetSessionUsernameAsync();
            if (!string.IsNullOrEmpty(sessionUser))
                UserName = sessionUser;

            var summary = await _transactionService.GetDashboardSummaryAsync();
            TotalBalance = summary.TotalBalance;
            TotalIncome = summary.TotalIncome;
            TotalExpense = summary.TotalExpense;
            CurrentMonthIncome = summary.CurrentMonthIncome;
            CurrentMonthExpense = summary.CurrentMonthExpense;

            TotalBalanceText = TotalBalance.ToString(AppConstants.CurrencyFormat);
            TotalIncomeText = TotalIncome.ToString(AppConstants.CurrencyFormat);
            TotalExpenseText = TotalExpense.ToString(AppConstants.CurrencyFormat);

            var savings = TotalIncome - TotalExpense;
            SavingsText = savings.ToString(AppConstants.CurrencyFormat);
            SavingsPercentText = TotalIncome > 0
                ? $"{(int)(savings / TotalIncome * 100)}%"
                : "0%";

            BalanceColor = TotalBalance >= 0 ? Color.FromArgb("#43A047") : Color.FromArgb("#E53935");

            var year = int.TryParse(SelectedYear, out var value) ? value : DateTime.Now.Year;

            MonthlyTrends.Clear();
            foreach (var point in await _transactionService.GetMonthlyTrendAsync(year))
            {
                MonthlyTrends.Add(point);
            }

            CategoryBreakdown.Clear();
            foreach (var item in await _transactionService.GetCategoryBreakdownAsync(DateTime.Now.Year, DateTime.Now.Month))
            {
                CategoryBreakdown.Add(item);
            }
        }
        finally
        {
            IsLoading = false;
            IsBusy = false;
        }
    }
}
