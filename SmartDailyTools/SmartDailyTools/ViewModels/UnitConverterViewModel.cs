using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using SmartDailyTools.Services;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class UnitConverterViewModel : BaseViewModel
{
    private readonly IUnitConverterService _service;
    private readonly IDatabaseService _db;

    [ObservableProperty] private string _selectedCategory = "Length";
    [ObservableProperty] private double _inputValue;
    [ObservableProperty] private string _selectedFromUnit = "Meter";
    [ObservableProperty] private string _selectedToUnit = "Kilometer";
    [ObservableProperty] private string _result = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<string> _availableUnits = [];
    [ObservableProperty] private ObservableCollection<UnitConverterHistory> _history = [];

    public List<string> Categories { get; }

    public UnitConverterViewModel(IUnitConverterService service, IDatabaseService db)
    {
        Title = "Unit Converter";
        _service = service;
        _db = db;
        Categories = service.GetCategories();
        LoadUnitsForCategory(_selectedCategory);
    }

    partial void OnSelectedCategoryChanged(string value)
    {
        LoadUnitsForCategory(value);
        Result = string.Empty;
        HasResult = false;
    }

    private void LoadUnitsForCategory(string category)
    {
        var units = _service.GetUnitsForCategory(category);
        AvailableUnits = new ObservableCollection<string>(units);
        SelectedFromUnit = units.FirstOrDefault() ?? string.Empty;
        SelectedToUnit = units.Count > 1 ? units[1] : units.FirstOrDefault() ?? string.Empty;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetUnitConverterHistoryAsync();
        History = new ObservableCollection<UnitConverterHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Convert()
    {
        if (string.IsNullOrEmpty(SelectedFromUnit) || string.IsNullOrEmpty(SelectedToUnit)) return;

        double output = _service.Convert(SelectedCategory, InputValue, SelectedFromUnit, SelectedToUnit);
        Result = $"{output:G9} {SelectedToUnit}";
        HasResult = true;
    }

    [RelayCommand]
    private void SwapUnits()
    {
        (SelectedFromUnit, SelectedToUnit) = (SelectedToUnit, SelectedFromUnit);
        if (HasResult) Convert();
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        double output = _service.Convert(SelectedCategory, InputValue, SelectedFromUnit, SelectedToUnit);
        var record = new UnitConverterHistory
        {
            Category = SelectedCategory,
            InputValue = InputValue,
            FromUnit = SelectedFromUnit,
            OutputValue = output,
            ToUnit = SelectedToUnit,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveUnitConverterHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "Conversion saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(UnitConverterHistory item)
    {
        await _db.DeleteUnitConverterHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        InputValue = 0;
        Result = string.Empty;
        HasResult = false;
    }
}
