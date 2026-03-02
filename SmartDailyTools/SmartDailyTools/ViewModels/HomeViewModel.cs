using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartDailyTools.Helpers;
using SmartDailyTools.Models;
using System.Collections.ObjectModel;

namespace SmartDailyTools.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    [ObservableProperty]
    private ObservableCollection<ToolItem> _tools = [];

    public HomeViewModel()
    {
        Title = "Smart Daily Tools";
        LoadTools();
    }

    private void LoadTools()
    {
        Tools =
        [
            new ToolItem
            {
                Title = "BMI Calculator",
                Description = "Check your Body Mass Index",
                Icon = "🏋️",
                Route = AppConstants.RouteBmi,
                GradientStart = "#4A6CF7",
                GradientEnd = "#6A3DE8"
            },
            new ToolItem
            {
                Title = "Age Calculator",
                Description = "Find your exact age",
                Icon = "🎂",
                Route = AppConstants.RouteAge,
                GradientStart = "#F59E0B",
                GradientEnd = "#D97706"
            },
            new ToolItem
            {
                Title = "EMI Calculator",
                Description = "Plan your loan EMI",
                Icon = "🏦",
                Route = AppConstants.RouteEmi,
                GradientStart = "#10B981",
                GradientEnd = "#059669"
            },
            new ToolItem
            {
                Title = "Percentage",
                Description = "Quick percentage calculations",
                Icon = "📊",
                Route = AppConstants.RoutePercentage,
                GradientStart = "#EF4444",
                GradientEnd = "#DC2626"
            },
            new ToolItem
            {
                Title = "GST Calculator",
                Description = "Calculate GST amounts",
                Icon = "🧾",
                Route = AppConstants.RouteGst,
                GradientStart = "#8B5CF6",
                GradientEnd = "#7C3AED"
            },
            new ToolItem
            {
                Title = "Unit Converter",
                Description = "Length, Weight, Temperature",
                Icon = "🔄",
                Route = AppConstants.RouteUnitConverter,
                GradientStart = "#06B6D4",
                GradientEnd = "#0891B2"
            },
            new ToolItem
            {
                Title = "QR Generator",
                Description = "Generate QR codes instantly",
                Icon = "📱",
                Route = AppConstants.RouteQr,
                GradientStart = "#F97316",
                GradientEnd = "#EA580C"
            },
            new ToolItem
            {
                Title = "Expense Splitter",
                Description = "Split bills easily",
                Icon = "💰",
                Route = AppConstants.RouteExpenseSplitter,
                GradientStart = "#EC4899",
                GradientEnd = "#DB2777"
            }
        ];
    }

    [RelayCommand]
    private async Task OpenToolAsync(ToolItem tool)
    {
        if (string.IsNullOrEmpty(tool.Route)) return;
        await Shell.Current.GoToAsync(tool.Route);
    }

}
