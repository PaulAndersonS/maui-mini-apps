using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class ReportsPage : ContentView
{
    private ReportsViewModel? ViewModel => BindingContext as ReportsViewModel;

    public ReportsPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService(typeof(ReportsViewModel));
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        if (ViewModel?.LoadSummaryCommand is not null)
        {
            await ViewModel.LoadSummaryCommand.ExecuteAsync(null);
        }
    }
}
