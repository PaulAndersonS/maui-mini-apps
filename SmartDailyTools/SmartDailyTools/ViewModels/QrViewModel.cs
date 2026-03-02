using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Data;
using SmartDailyTools.Models;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class QrViewModel : BaseViewModel
{
    private readonly IDatabaseService _db;

    [ObservableProperty] private string _inputText = string.Empty;
    // Must never be empty — SfBarcodeGenerator throws ArgumentException on empty Value.
    [ObservableProperty] private string _qrValue = " ";
    [ObservableProperty] private bool _hasQr;
    [ObservableProperty] private ObservableCollection<QrHistory> _history = [];

    public QrViewModel(IDatabaseService db)
    {
        Title = "QR Code Generator";
        _db = db;
    }

    [RelayCommand]
    private async Task AppearingAsync()
    {
        IsLoading = true;
        var records = await _db.GetQrHistoryAsync();
        History = new ObservableCollection<QrHistory>(records);
        IsLoading = false;
    }

    [RelayCommand]
    private void Generate()
    {
        if (string.IsNullOrWhiteSpace(InputText)) return;
        QrValue = InputText.Trim();
        HasQr = true;
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (!HasQr) return;

        var record = new QrHistory
        {
            InputText = QrValue,
            CreatedAt = DateTime.UtcNow
        };

        await _db.SaveQrHistoryAsync(record);
        History.Insert(0, record);
        await Shell.Current.DisplayAlertAsync("Saved", "QR record saved to history.", "OK");
    }

    [RelayCommand]
    private async Task RegenerateFromHistoryAsync(QrHistory item)
    {
        InputText = item.InputText;
        QrValue = item.InputText;
        HasQr = true;
        await Shell.Current.DisplayAlertAsync("Loaded", "QR code regenerated from history.", "OK");
    }

    [RelayCommand]
    private async Task DeleteHistoryAsync(QrHistory item)
    {
        await _db.DeleteQrHistoryAsync(item);
        History.Remove(item);
    }

    [RelayCommand]
    private void Reset()
    {
        InputText = string.Empty;
        QrValue = " "; // keep non-empty to avoid SfBarcodeGenerator crash
        HasQr = false;
    }
}
