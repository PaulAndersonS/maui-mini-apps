using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class PercentageViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;

    [ObservableProperty] private string _selectedMode = "X% of Y";
    [ObservableProperty] private double _inputA;
    [ObservableProperty] private double _inputB;
    [ObservableProperty] private string _result = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private string _labelA = "Percentage (%)";
    [ObservableProperty] private string _labelB = "Value";
    [ObservableProperty] private ObservableCollection<PercentageHistory> _history = [];

    public List<string> Modes { get; } = ["X% of Y", "What % is X of Y", "% Increase", "% Decrease"];

    public PercentageViewModel(IDatabaseService db)
    {
        Title = "Percentage Calculator";
        _db = db;
    }

    partial void OnSelectedModeChanged(string value)
    {
        UpdateLabels();
        Reset();
    }

    private void UpdateLabels()
    {
        switch (SelectedMode)
        {
            case "X% of Y":
                LabelA = "Percentage (X%)";
                LabelB = "Value (Y)";
                break;
            case "What % is X of Y":
                LabelA = "Value (X)";
                LabelB = "Total (Y)";
                break;
            case "% Increase":
            case "% Decrease":
                LabelA = "Original Value";
                LabelB = "New Value";
                break;
        }
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetPercentageHistoryAsync();
        History = new ObservableCollection<PercentageHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        double res = SelectedMode switch
        {
            "X% of Y"          => (InputA / 100.0) * InputB,
            "What % is X of Y" => InputB == 0 ? 0 : (InputA / InputB) * 100.0,
            "% Increase"       => InputA == 0 ? 0 : ((InputB - InputA) / InputA) * 100.0,
            "% Decrease"       => InputA == 0 ? 0 : ((InputA - InputB) / InputA) * 100.0,
            _                  => 0
        };

        Result = SelectedMode.Contains('%') && !SelectedMode.Contains("of")
            ? $"{res:N2}%"
            : $"{res:N4}";
        HasResult = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        double res = SelectedMode switch
        {
            "X% of Y"          => (InputA / 100.0) * InputB,
            "What % is X of Y" => InputB == 0 ? 0 : (InputA / InputB) * 100.0,
            "% Increase"       => InputA == 0 ? 0 : ((InputB - InputA) / InputA) * 100.0,
            "% Decrease"       => InputA == 0 ? 0 : ((InputA - InputB) / InputA) * 100.0,
            _                  => 0
        };

        var record = new PercentageHistory
        {
            Mode = SelectedMode,
            InputA = InputA,
            InputB = InputB,
            Result = res,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SavePercentageHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "Record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(PercentageHistory item)
    {
        await _db.DeletePercentageHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        InputA = 0;
        InputB = 0;
        Result = string.Empty;
        HasResult = false;
    }
}
