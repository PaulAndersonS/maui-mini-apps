using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class UnitConverterPage : ContentPage
{
    private readonly UnitConverterViewModel _vm;
    public UnitConverterPage(UnitConverterViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
