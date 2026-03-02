using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class TransactionsPage : ContentView
{
    private TransactionsViewModel? ViewModel => BindingContext as TransactionsViewModel;

    public TransactionsPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService(typeof(TransactionsViewModel));
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        if (ViewModel?.LoadCommand is not null)
        {
            await ViewModel.LoadCommand.ExecuteAsync(null);
        }
    }
}
