using CommunityToolkit.Mvvm.Messaging;
using SmartExpense.Helpers;
using SmartExpense.ViewModels;

namespace SmartExpense;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        WeakReferenceMessenger.Default.Register<NavigateToTabMessage>(this, (_, msg) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                MainTabView.SelectedIndex = msg.TabIndex;
            });
        });
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        WeakReferenceMessenger.Default.Unregister<NavigateToTabMessage>(this);
    }
}
