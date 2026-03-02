using Microsoft.Extensions.DependencyInjection;
using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        viewModel.RegisterSucceeded += () =>
        {
            var mainPage = Handler?.MauiContext?.Services.GetRequiredService<MainPage>();
            if (mainPage is not null)
                Application.Current!.Windows[0].Page = mainPage;
        };

        viewModel.NavigateToLogin += async () =>
        {
            await Navigation.PopAsync();
        };
    }
}
