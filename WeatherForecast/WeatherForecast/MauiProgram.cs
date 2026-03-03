using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;
using Syncfusion.Maui.Core.Hosting;
using WeatherForecast.Services;
using WeatherForecast.ViewModels;
using WeatherForecast.Views;

namespace WeatherForecast;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // ── Syncfusion license registration ──────────────────────────────────
        // Replace the placeholder below with your free Syncfusion license key.
        // Obtain one at: https://www.syncfusion.com/sales/communitylicense
        // Wrapped in try/catch: an invalid/placeholder key throws
        // SyncfusionLicenseValidationException which would otherwise crash
        // CreateMauiApp() and permanently freeze the app on the splash screen.
        try
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "Add your Syncfusion license key here");
        }
        catch (Exception)
        {
            // License validation failed – controls will show a trial watermark
            // but the app will still launch.  Replace the key above to silence this.
        }

        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            // ── Register all Syncfusion MAUI control handlers ───────────────
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf",   "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf",  "OpenSansSemibold");
            });

        // ── HTTP client for Open-Meteo (no API key required) ─────────────────
        builder.Services.AddHttpClient<WeatherService>(client =>
        {
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
                    "application/json"));
        });

        // ── Application services ──────────────────────────────────────────────
        builder.Services.AddTransient<WeatherViewModel>();
        builder.Services.AddTransient<WeatherDashboardPage>();
        builder.Services.AddTransient<AppShell>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
