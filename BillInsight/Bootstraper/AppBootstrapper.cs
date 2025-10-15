using BillInsight.Services;
using Splat;

namespace BillInsight.Bootstraper
{
    public class AppBootstrapper : IEnableLogger
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(NavigationService));
            Locator.CurrentMutable.RegisterConstant(new InvoiceDataService(), typeof(InvoiceDataService));
            Locator.CurrentMutable.RegisterConstant(new BachHoaXanhService(), typeof(BachHoaXanhService));
            Locator.CurrentMutable.RegisterConstant(new GoogleSpreadsheetService(), typeof(GoogleSpreadsheetService));
        }
    }
}