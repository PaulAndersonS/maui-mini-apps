using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;
using SmartDailyTools.Data;
using SmartDailyTools.Helpers;
using SmartDailyTools.Services;
using SmartDailyTools.ViewModels;
using SmartDailyTools.Views;

namespace SmartDailyTools;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // Register Syncfusion license — replace with your actual key
        Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(AppConstants.SyncfusionLicenseKey);

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // ── Database ─────────────────────────────────────────────────────────
        builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

        // ── Services ─────────────────────────────────────────────────────────
        builder.Services.AddSingleton<IBmiService, BmiService>();
        builder.Services.AddSingleton<IAgeService, AgeService>();
        builder.Services.AddSingleton<IEmiService, EmiService>();
        builder.Services.AddSingleton<IUnitConverterService, UnitConverterService>();

        // ── ViewModels ───────────────────────────────────────────────────────
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddTransient<BmiViewModel>();
        builder.Services.AddTransient<AgeViewModel>();
        builder.Services.AddTransient<EmiViewModel>();
        builder.Services.AddTransient<PercentageViewModel>();
        builder.Services.AddTransient<GstViewModel>();
        builder.Services.AddTransient<UnitConverterViewModel>();
        builder.Services.AddTransient<QrViewModel>();
        builder.Services.AddTransient<ExpenseSplitterViewModel>();

        // ── Pages ────────────────────────────────────────────────────────────
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<BmiPage>();
        builder.Services.AddTransient<AgePage>();
        builder.Services.AddTransient<EmiPage>();
        builder.Services.AddTransient<PercentagePage>();
        builder.Services.AddTransient<GstPage>();
        builder.Services.AddTransient<UnitConverterPage>();
        builder.Services.AddTransient<QrPage>();
        builder.Services.AddTransient<ExpenseSplitterPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<AnalyticsPage>();
        builder.Services.AddTransient<WalletPage>();
        builder.Services.AddTransient<SettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // Initialise the database tables on startup
        Task.Run(async () =>
        {
            var db = app.Services.GetRequiredService<IDatabaseService>();
            await db.InitializeAsync();
        });

        return app;
    }
}

