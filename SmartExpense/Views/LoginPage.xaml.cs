using Microsoft.Extensions.DependencyInjection;
using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.LoginSucceeded += async () =>
        {
            var mainPage = Handler?.MauiContext?.Services.GetRequiredService<MainPage>();
            if (mainPage is not null)
                Application.Current!.Windows[0].Page = mainPage;
        };

        viewModel.NavigateToRegister += async () =>
        {
            var register = Handler?.MauiContext?.Services.GetRequiredService<RegisterPage>();
            if (register is not null)
                await Navigation.PushAsync(register);
        };
    }
}
