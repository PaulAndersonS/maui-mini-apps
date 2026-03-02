using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class GstPage : ContentPage
{
    private readonly GstViewModel _vm;
    public GstPage(GstViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
