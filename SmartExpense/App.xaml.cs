using Microsoft.Extensions.DependencyInjection;
using SmartExpense.Services;
using SmartExpense.Views;

namespace SmartExpense;

public partial class App : Application
{
	private readonly IServiceProvider _serviceProvider;

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_serviceProvider = serviceProvider;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Return a temporary window; real startup page is set asynchronously
		var nav = new NavigationPage();
		var window = new Window(nav);
		Task.Run(() => InitializeStartupAsync(nav));
		return window;
	}

	private async Task InitializeStartupAsync(NavigationPage nav)
	{
		var authService = _serviceProvider.GetRequiredService<IAuthService>();
		bool hasSession = await authService.IsSessionActiveAsync();

		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			if (hasSession)
			{
				Windows[0].Page = _serviceProvider.GetRequiredService<MainPage>();
			}
			else
			{
				var loginPage = _serviceProvider.GetRequiredService<LoginPage>();
				Windows[0].Page = new NavigationPage(loginPage);
			}
		});
	}
}