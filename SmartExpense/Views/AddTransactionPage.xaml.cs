using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class AddTransactionPage : ContentView
{
    private AddTransactionViewModel? ViewModel => BindingContext as AddTransactionViewModel;

    public AddTransactionPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService(typeof(AddTransactionViewModel));
        Loaded += OnLoaded;
        BindingContextChanged += OnBindingContextChanged;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await EnsureCategoriesLoadedAsync();
    }

    private async void OnBindingContextChanged(object? sender, EventArgs e)
    {
        await EnsureCategoriesLoadedAsync();
    }

    private async Task EnsureCategoriesLoadedAsync()
    {
        if (ViewModel?.LoadCategoriesCommand is null)
        {
            return;
        }

        if (ViewModel.Categories.Count == 0)
        {
            await ViewModel.LoadCategoriesCommand.ExecuteAsync(null);
        }
    }
}
