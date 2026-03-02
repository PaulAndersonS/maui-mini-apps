using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using SmartDailyTools.Services;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class BmiViewModel : BaseViewModel
{
    private readonly IBmiService _bmiService;
    private readonly IDatabaseService _db;

    [ObservableProperty] private double _weight = 70;
    [ObservableProperty] private double _height = 170;
    [ObservableProperty] private string _selectedWeightUnit = "kg";
    [ObservableProperty] private string _selectedHeightUnit = "cm";
    [ObservableProperty] private string _bmiResult = string.Empty;
    [ObservableProperty] private string _bmiCategory = string.Empty;
    [ObservableProperty] private Color _categoryColor = Colors.Transparent;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<BmiHistory> _history = [];

// Chart data
    [ObservableProperty] private ObservableCollection<ChartDataPoint> _chartData = [];

    // Circular chart — fixed BMI range segments used to show the user's category
    public List<BmiCategoryDataPoint> BmiCategoryItems { get; } =
    [
        new("Underweight\n< 18.5",  18.5, Color.FromArgb("#3B82F6")),
        new("Normal\n18.5–24.9",     6.5, Color.FromArgb("#22C55E")),
        new("Overweight\n25–29.9",   5.0, Color.FromArgb("#F59E0B")),
        new("Obese\n≥ 30",          10.0, Color.FromArgb("#EF4444"))
    ];

    public List<string> WeightUnits { get; } = ["kg", "lbs"];
    public List<string> HeightUnits { get; } = ["cm", "m", "ft", "in"];

    public BmiViewModel(IBmiService bmiService, IDatabaseService db)
    {
        Title = "BMI Calculator";
        _bmiService = bmiService;
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetBmiHistoryAsync();
        History = new ObservableCollection<BmiHistory>(records);
        BuildTrendData(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        if (Weight <= 0 || Height <= 0) return;

        var bmi = _bmiService.Calculate(Weight, Height, SelectedWeightUnit, SelectedHeightUnit);
        var category = _bmiService.GetCategory(bmi);
        var color = _bmiService.GetCategoryColor(category);

        BmiResult = bmi.ToString("F1");
        BmiCategory = category;
        CategoryColor = color;
        HasResult = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        var record = new BmiHistory
        {
            Weight = Weight,
            WeightUnit = SelectedWeightUnit,
            Height = Height,
            HeightUnit = SelectedHeightUnit,
            BmiValue = double.Parse(BmiResult),
            Category = BmiCategory,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveBmiHistoryAsync(record);
        History.Insert(0, record);
        BuildTrendData([.. History]);
        await Shell.Current.DisplayAlertAsync("Saved", "BMI record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(BmiHistory item)
    {
        await _db.DeleteBmiHistoryAsync(item);
        History.Remove(item);
        BuildTrendData([.. History]);
    }

    [RelayCommand]
    private void Reset()
    {
        Weight = 70;
        Height = 170;
        BmiResult = string.Empty;
        BmiCategory = string.Empty;
        CategoryColor = Colors.Transparent;
        HasResult = false;
    }

    private void BuildTrendData(List<BmiHistory> records)
    {
        ChartData = new ObservableCollection<ChartDataPoint>(
            records
                .OrderBy(x => x.CreatedAt)
                .TakeLast(10)
                .Select(x => new ChartDataPoint(x.CreatedAt.ToLocalTime().ToString("dd MMM"), x.BmiValue)));
    }
}

public record ChartDataPoint(string Label, double Value);
public record BmiCategoryDataPoint(string Name, double Value, Color ItemColor);
