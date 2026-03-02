using SmartDailyTools.Data;

namespace SmartDailyTools.Views;

public partial class SettingsPage : ContentPage
{
    private readonly IDatabaseService _db;

    public SettingsPage(IDatabaseService db)
    {
        InitializeComponent();
        _db = db;
    }

    private async void OnClearHistoryClicked(object? sender, EventArgs e)
    {
        bool confirmed = await DisplayAlertAsync(
            "Clear History",
            "Are you sure you want to delete all saved history? This cannot be undone.",
            "Clear", "Cancel");

        if (!confirmed) return;

        await _db.ClearAllHistoryAsync();
        await DisplayAlertAsync("Done", "All history has been cleared.", "OK");
    }
}
