using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class QrPage : ContentPage
{
    private readonly QrViewModel _vm;
    public QrPage(QrViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
