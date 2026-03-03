namespace WeatherForecast;

public partial class App : Application
{
    private readonly AppShell _shell;

    /// <summary>
    /// AppShell is resolved from the DI container by the MAUI host,
    /// which in turn resolves WeatherDashboardPage and WeatherViewModel.
    /// </summary>
    public App(AppShell shell)
    {
        InitializeComponent();
        _shell = shell;
    }

    protected override Window CreateWindow(IActivationState? activationState)
        => new Window(_shell);
}