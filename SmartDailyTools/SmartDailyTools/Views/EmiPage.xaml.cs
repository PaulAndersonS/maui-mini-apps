using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class EmiPage : ContentPage
{
    private readonly EmiViewModel _vm;
    public EmiPage(EmiViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
