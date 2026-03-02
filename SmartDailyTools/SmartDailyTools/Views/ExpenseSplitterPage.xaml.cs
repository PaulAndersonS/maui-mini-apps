using SmartDailyTools.ViewModels;

namespace SmartDailyTools.Views;

public partial class ExpenseSplitterPage : ContentPage
{
    private readonly ExpenseSplitterViewModel _vm;
    public ExpenseSplitterPage(ExpenseSplitterViewModel vm) { InitializeComponent(); _vm = vm; BindingContext = vm; }
    protected override async void OnAppearing() { base.OnAppearing(); await _vm.AppearingCommand.ExecuteAsync(null); }
}
