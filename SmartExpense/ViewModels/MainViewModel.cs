using CommunityToolkit.Mvvm.Input;
using SmartExpense.Services;

namespace SmartExpense.ViewModels;

public partial class MainViewModel(IThemeService themeService) : BaseViewModel
{
    private readonly IThemeService _themeService = themeService;

    [RelayCommand]
    private void ToggleTheme()
    {
        _themeService.ToggleTheme();
    }
}
