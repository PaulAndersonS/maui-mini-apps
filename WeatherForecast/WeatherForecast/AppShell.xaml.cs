using Microsoft.Extensions.DependencyInjection;
using WeatherForecast.Views;

namespace WeatherForecast;

public partial class AppShell : Shell
{
    /// <summary>
    /// Accepts IServiceProvider rather than WeatherDashboardPage directly.
    /// Using ContentTemplate defers page construction until the first navigation,
    /// by which point the Android Activity (and its ContentResolver) is fully
    /// initialised.  Injecting the page directly caused SfCartesianChart to call
    /// Settings.Global.GetFloat(ContentResolver) during Application.OnCreate()
    /// when ContentResolver is still null, producing a NullPointerException.
    /// </summary>
    public AppShell(IServiceProvider services)
    {
        InitializeComponent();

        Items.Add(new ShellContent
        {
            Title = "Dashboard",
            Route = "WeatherDashboard",
            ContentTemplate = new DataTemplate(
                () => services.GetRequiredService<WeatherDashboardPage>())
        });
    }
}
