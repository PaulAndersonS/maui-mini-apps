using SmartDailyTools.ViewModels;
using Syncfusion.Maui.Inputs;

namespace SmartDailyTools.Views;

public partial class BmiPage : ContentPage
{
    private readonly BmiViewModel _vm;

    public BmiPage(BmiViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _vm.AppearingCommand.ExecuteAsync(null);
    }

    // Guarantee the ViewModel is updated immediately on each keystroke,
    // regardless of whether SfNumericEntry has committed its TwoWay binding.
    private void OnWeightChanged(object? sender, NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue.HasValue && e.NewValue.Value > 0)
            _vm.Weight = e.NewValue.Value;
    }

    private void OnHeightChanged(object? sender, NumericEntryValueChangedEventArgs e)
    {
        if (e.NewValue.HasValue && e.NewValue.Value > 0)
            _vm.Height = e.NewValue.Value;
    }
}
