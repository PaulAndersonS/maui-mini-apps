using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpense.Services;

namespace SmartExpense.ViewModels;

public partial class RegisterViewModel(IAuthService authService) : BaseViewModel
{
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string confirmPassword = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public event Action? RegisterSucceeded;
    public event Action? NavigateToLogin;

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;
        if (IsBusy) return;

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return;
        }

        IsBusy = true;
        try
        {
            var (success, error) = await authService.RegisterAsync(Username, Password);
            if (success)
                RegisterSucceeded?.Invoke();
            else
                ErrorMessage = error;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GoToLogin() => NavigateToLogin?.Invoke();
}
