using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartExpense.Services;

namespace SmartExpense.ViewModels;

public partial class LoginViewModel(IAuthService authService) : BaseViewModel
{
    [ObservableProperty]
    private string username = string.Empty;

    [ObservableProperty]
    private string password = string.Empty;

    [ObservableProperty]
    private string errorMessage = string.Empty;

    public event Action? LoginSucceeded;
    public event Action? NavigateToRegister;

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;
        if (IsBusy) return;
        IsBusy = true;
        try
        {
            var (success, error) = await authService.LoginAsync(Username, Password);
            if (success)
                LoginSucceeded?.Invoke();
            else
                ErrorMessage = error;
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void GoToRegister() => NavigateToRegister?.Invoke();
}
