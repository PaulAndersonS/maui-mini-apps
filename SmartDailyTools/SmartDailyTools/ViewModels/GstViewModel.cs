using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class GstViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;

    [ObservableProperty] private double _baseAmount;
    [ObservableProperty] private double _selectedGstRate = 18;
    [ObservableProperty] private string _gstAmount = string.Empty;
    [ObservableProperty] private string _totalAmount = string.Empty;
    [ObservableProperty] private bool _hasResult;
    [ObservableProperty] private ObservableCollection<GstHistory> _history = [];
    [ObservableProperty] private ObservableCollection<ChartDataPoint> _gstBreakdownData = [];

    public List<double> GstRates { get; } = [0, 0.1, 0.25, 3, 5, 12, 18, 28];

    public GstViewModel(IDatabaseService db)
    {
        Title = "GST Calculator";
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetGstHistoryAsync();
        History = new ObservableCollection<GstHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Calculate()
    {
        if (BaseAmount <= 0) return;

        double gst = BaseAmount * SelectedGstRate / 100.0;
        double total = BaseAmount + gst;

        GstAmount = $"₹{gst:N2}";
        TotalAmount = $"₹{total:N2}";
        HasResult = true;

        GstBreakdownData =
        [
            new ChartDataPoint("Base", BaseAmount),
            new ChartDataPoint("GST", gst)
        ];
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasResult) return;

        double gst = BaseAmount * SelectedGstRate / 100.0;
        var record = new GstHistory
        {
            BaseAmount = BaseAmount,
            GstPercent = SelectedGstRate,
            GstAmount = gst,
            TotalAmount = BaseAmount + gst,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveGstHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "GST record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(GstHistory item)
    {
        await _db.DeleteGstHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        BaseAmount = 0;
        SelectedGstRate = 18;
        GstAmount = string.Empty;
        TotalAmount = string.Empty;
        HasResult = false;
        GstBreakdownData = [];
    }
}
