using SmartDailyTools.Data;
using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class HomePage : ContentPage
{
    private readonly IDatabaseService _db;

    public HomePage(HomeViewModel vm, IDatabaseService db)
    {
        InitializeComponent();
        BindingContext = vm;
        _db = db;
    }

    private async void OnMenuTapped(object? sender, TappedEventArgs e)
    {
        const string clearHistory  = "🗑  Clear All History";
        const string about         = "ℹ️  About";

        string? action = await DisplayActionSheet(
            "Smart Daily Tools",
            "Cancel",
            null,
            clearHistory,
            about);

        if (action == clearHistory)
        {
            bool confirmed = await DisplayAlertAsync(
                "Clear History",
                "Delete all saved calculation records? This cannot be undone.",
                "Clear", "Cancel");

            if (!confirmed) return;
            await _db.ClearAllHistoryAsync();
            await DisplayAlertAsync("Done", "All history cleared.", "OK");
        }
        else if (action == about)
        {
            await DisplayAlertAsync(
                "About",
                "Smart Daily Tools v1.0\nBuilt with .NET MAUI & Syncfusion\n\n8 free daily calculators — always offline.",
                "OK");
        }
    }
}
