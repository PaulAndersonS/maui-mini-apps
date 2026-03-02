using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class DashboardPage : ContentView
{
    private DashboardViewModel? ViewModel => BindingContext as DashboardViewModel;

    public DashboardPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService(typeof(DashboardViewModel));
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        if (Opacity == 1)
        {
            Opacity = 0;
            await this.FadeToAsync(1, 320, Easing.CubicOut);
        }

        if (ViewModel?.LoadCommand is not null)
        {
            await ViewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
