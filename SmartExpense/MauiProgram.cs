using SmartExpense.Data;
using SmartExpense.Helpers;
using SmartExpense.Services;
using SmartExpense.ViewModels;
using SmartExpense.Views;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;
using Microsoft.Extensions.Logging;

namespace SmartExpense;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureSyncfusionCore()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		SyncfusionLicenseProvider.RegisterLicense(AppConstants.SyncfusionLicenseKey);

		builder.Services.AddSingleton<ExpenseDbContext>();
		builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
		builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
		builder.Services.AddSingleton<IUserRepository, UserRepository>();
		builder.Services.AddSingleton<IAuthService, AuthService>();
		builder.Services.AddSingleton<ITransactionService, TransactionService>();
		builder.Services.AddSingleton<IReportService, ReportService>();
		builder.Services.AddSingleton<IThemeService, ThemeService>();
#if ANDROID
		builder.Services.AddSingleton<IDeviceAIService, SmartExpense.Platforms.Android.DeviceAIService>();
#else
		builder.Services.AddSingleton<IDeviceAIService, DefaultDeviceAIService>();
#endif

		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<RegisterViewModel>();
		builder.Services.AddTransient<MainViewModel>();
		builder.Services.AddTransient<DashboardViewModel>();
		builder.Services.AddTransient<AddTransactionViewModel>();
		builder.Services.AddTransient<TransactionsViewModel>();
		builder.Services.AddTransient<ReportsViewModel>();
		builder.Services.AddTransient<AboutViewModel>();

		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<RegisterPage>();
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<DashboardPage>();
		builder.Services.AddTransient<AddTransactionPage>();
		builder.Services.AddTransient<TransactionsPage>();
		builder.Services.AddTransient<ReportsPage>();
		builder.Services.AddTransient<AboutPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
