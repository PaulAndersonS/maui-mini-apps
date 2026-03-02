using SmartDailyTools.Helpers;
using SmartDailyTools.Views;

namespace SmartDailyTools;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        RegisterRoutes();
    }

    private static void RegisterRoutes()
    {
        Routing.RegisterRoute(AppConstants.RouteBmi,              typeof(BmiPage));
        Routing.RegisterRoute(AppConstants.RouteAge,              typeof(AgePage));
        Routing.RegisterRoute(AppConstants.RouteEmi,              typeof(EmiPage));
        Routing.RegisterRoute(AppConstants.RoutePercentage,       typeof(PercentagePage));
        Routing.RegisterRoute(AppConstants.RouteGst,              typeof(GstPage));
        Routing.RegisterRoute(AppConstants.RouteUnitConverter,    typeof(UnitConverterPage));
        Routing.RegisterRoute(AppConstants.RouteQr,               typeof(QrPage));
        Routing.RegisterRoute(AppConstants.RouteExpenseSplitter,  typeof(ExpenseSplitterPage));
    }
}

