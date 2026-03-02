using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class AgePage : ContentPage
{
    private readonly AgeViewModel _vm;
    public AgePage(AgeViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
