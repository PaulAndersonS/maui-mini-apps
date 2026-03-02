using SmartExpense.ViewModels;

namespace SmartExpense.Views;

public partial class AboutPage : ContentView
{
    public AboutPage()
    {
        InitializeComponent();
        BindingContext = IPlatformApplication.Current?.Services.GetService(typeof(AboutViewModel));
    }
}
