using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class PercentagePage : ContentPage
{
    private readonly PercentageViewModel _vm;
    public PercentagePage(PercentageViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
