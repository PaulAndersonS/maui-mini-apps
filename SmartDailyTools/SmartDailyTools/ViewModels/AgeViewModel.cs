using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using SmartDailyTools.Services;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class AgeViewModel : BaseViewModel
{
    private readonly IAgeService _ageService;
    private readonly IDatabaseService _db;

    [ObservableProperty] private DateTime _dateOfBirth = new(1990, 1, 1);
    [ObservableProperty] private string _years = string.Empty;
    [ObservableProperty] private string _months = string.Empty;
    [ObservableProperty] private string _days = string.Empty;
    [ObservableProperty] private string _totalDays = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<AgeHistory> _history = [];

    public DateTime MaxDate { get; } = DateTime.Today;
    public DateTime MinDate { get; } = new DateTime(1900, 1, 1);

    public AgeViewModel(IAgeService ageService, IDatabaseService db)
    {
        Title = "Age Calculator";
        _ageService = ageService;
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetAgeHistoryAsync();
        History = new ObservableCollection<AgeHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        var result = _ageService.Calculate(DateOfBirth);
        Years = result.Years.ToString();
        Months = result.Months.ToString();
        Days = result.Days.ToString();
        TotalDays = result.TotalDays.ToString("N0");
        HasResult = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        var result = _ageService.Calculate(DateOfBirth);
        var record = new AgeHistory
        {
            DateOfBirth = DateOfBirth,
            Years = result.Years,
            Months = result.Months,
            Days = result.Days,
            TotalDays = result.TotalDays,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveAgeHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "Age record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(AgeHistory item)
    {
        await _db.DeleteAgeHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        DateOfBirth = new DateTime(1990, 1, 1);
        Years = string.Empty;
        Months = string.Empty;
        Days = string.Empty;
        TotalDays = string.Empty;
        HasResult = false;
    }
}
