using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class ExpenseSplitterViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;

    [ObservableProperty] private double _totalBill;
    [ObservableProperty] private double _numberOfPeople = 2;
    [ObservableProperty] private double _tipPercent;
    [ObservableProperty] private string _perPersonAmount = string.Empty;
    [ObservableProperty] private string _totalWithTip = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<ExpenseSplitterHistory> _history = [];

    public List<double> TipOptions { get; } = [0, 5, 10, 15, 18, 20, 25];

    public ExpenseSplitterViewModel(IDatabaseService db)
    {
        Title = "Expense Splitter";
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetExpenseSplitterHistoryAsync();
        History = new ObservableCollection<ExpenseSplitterHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        if (TotalBill <= 0 || NumberOfPeople <= 0) return;

        double tip = TotalBill * TipPercent / 100.0;
        double total = TotalBill + tip;
        double perPerson = total / NumberOfPeople;

        TotalWithTip = $"₹{total:N2}";
        PerPersonAmount = $"₹{perPerson:N2}";
        HasResult = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        double tip = TotalBill * TipPercent / 100.0;
        double total = TotalBill + tip;
        double perPerson = total / NumberOfPeople;

        var record = new ExpenseSplitterHistory
        {
            TotalBill = TotalBill,
            NumberOfPeople = (int)NumberOfPeople,
            TipPercent = TipPercent,
            PerPersonAmount = perPerson,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveExpenseSplitterHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "Record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(ExpenseSplitterHistory item)
    {
        await _db.DeleteExpenseSplitterHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        TotalBill = 0;
        NumberOfPeople = 2;
        TipPercent = 0;
        PerPersonAmount = string.Empty;
        TotalWithTip = string.Empty;
        HasResult = false;
    }
}
