using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using SmartDailyTools.Services;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class EmiViewModel : BaseViewModel
{
    private readonly IEmiService _emiService;
    private readonly IDatabaseService _db;

    [ObservableProperty] private double _loanAmount;
    [ObservableProperty] private double _interestRate;
    [ObservableProperty] private double _tenure;
    [ObservableProperty] private string _selectedTenureType = "Months";
    [ObservableProperty] private string _emiResult = string.Empty;
    [ObservableProperty] private string _totalInterest = string.Empty;
    [ObservableProperty] private string _totalPayment = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<EmiHistory> _history = [];
    [ObservableProperty] private ObservableCollection<ChartDataPoint> _principalVsInterestData = [];

    public List<string> TenureTypes { get; } = ["Months", "Years"];

    public EmiViewModel(IEmiService emiService, IDatabaseService db)
    {
        Title = "EMI Calculator";
        _emiService = emiService;
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetEmiHistoryAsync();
        History = new ObservableCollection<EmiHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        if (LoanAmount <= 0 || InterestRate <= 0 || Tenure <= 0) return;

        int tenureMonths = SelectedTenureType == "Years" ? (int)(Tenure * 12) : (int)Tenure;
        var result = _emiService.Calculate(LoanAmount, InterestRate, tenureMonths);

        EmiResult = $"₹{result.Emi:N2}";
        TotalInterest = $"₹{result.TotalInterest:N2}";
        TotalPayment = $"₹{result.TotalPayment:N2}";
        HasResult = true;

        PrincipalVsInterestData =
        [
            new ChartDataPoint("Principal", LoanAmount),
            new ChartDataPoint("Interest", result.TotalInterest)
        ];
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        int tenureMonths = SelectedTenureType == "Years" ? (int)(Tenure * 12) : (int)Tenure;
        var result = _emiService.Calculate(LoanAmount, InterestRate, tenureMonths);

        var record = new EmiHistory
        {
            LoanAmount = LoanAmount,
            InterestRate = InterestRate,
            TenureMonths = tenureMonths,
            Emi = result.Emi,
            TotalInterest = result.TotalInterest,
            TotalPayment = result.TotalPayment,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveEmiHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "EMI record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(EmiHistory item)
    {
        await _db.DeleteEmiHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        LoanAmount = 0;
        InterestRate = 0;
        Tenure = 0;
        EmiResult = string.Empty;
        TotalInterest = string.Empty;
        TotalPayment = string.Empty;
        HasResult = false;
        PrincipalVsInterestData = [];
    }
}
