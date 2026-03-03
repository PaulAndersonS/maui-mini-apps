using WeatherForecast.ViewModels;

namespace WeatherForecast.Views;

public partial class WeatherDashboardPage : ContentPage
{
    public WeatherDashboardPage(WeatherViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // ── Lifecycle ────────────────────────────────────────────────────────────

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is WeatherViewModel vm)
        {
            vm.PropertyChanged += OnViewModelPropertyChanged;

            // Trigger the initial data load here, after the Android activity is
            // fully set up.  InitializeAsync is a no-op if data is already loaded.
            await vm.InitializeAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is WeatherViewModel vm)
        {
            vm.PropertyChanged -= OnViewModelPropertyChanged;
        }
    }

    // ── Animations ───────────────────────────────────────────────────────────

    private void OnViewModelPropertyChanged(object? sender,
        System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Soft fade-in when weather data arrives (IsLoading transitions to false)
        if (e.PropertyName is nameof(WeatherViewModel.IsLoading)
            && sender is WeatherViewModel vm
            && !vm.IsLoading)
        {
            _ = FadeInContentAsync();
        }
    }

    private async Task FadeInContentAsync()
    {
        this.Opacity = 0.7;
        await this.FadeToAsync(1.0, 350, Easing.CubicOut);
    }
}
