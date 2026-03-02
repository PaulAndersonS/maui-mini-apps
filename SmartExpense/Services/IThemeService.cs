namespace SmartExpense.Services;

public interface IThemeService
{
    AppTheme CurrentTheme { get; }
    void ToggleTheme();
}
