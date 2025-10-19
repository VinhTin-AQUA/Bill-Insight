using System.Threading.Tasks;
using BillInsight.Services;
using Splat;

namespace BillInsight.Bootstraper
{
    public class AppBootstrapper : IEnableLogger
    {
        public const string NavigationServiceName = "NavigationService";
        public const string InvoiceDataServiceName = "InvoiceDataService";
        public const string BachHoaXanhServiceName = "BachHoaXanhService";
        public const string GoogleSpreadsheetServiceName = "GoogleSpreadsheetService";
        public const string DialogServiceName = "DialogService";
        public const string ConfigServiceName = "ConfigService";
        
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(NavigationService));
            Locator.CurrentMutable.RegisterConstant(new InvoiceDataService(), typeof(InvoiceDataService));
            Locator.CurrentMutable.RegisterConstant(new BachHoaXanhService(), typeof(BachHoaXanhService));
            Locator.CurrentMutable.RegisterConstant(new GoogleSpreadsheetService(), typeof(GoogleSpreadsheetService));
            Locator.CurrentMutable.RegisterConstant(new DialogService(), typeof(DialogService));
            Locator.CurrentMutable.RegisterConstant(new ConfigService(), typeof(ConfigService));
            Locator.CurrentMutable.RegisterConstant(new WindowNavigationService(), typeof(WindowNavigationService));
        }

        public async Task<(bool, string)> InitAsync()
        {
            var configService = Locator.Current.GetService<ConfigService>()!;
            var checkConfigService = await configService.InitAsync();
            if (!checkConfigService)
            {
                return (false, ConfigServiceName);
            }
            
            var googleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;
            var checkGoogleSpreadsheetService = googleSpreadsheetService.Init(configService.Config.ServiceAccountCredentialFilePath, configService.Config.SpreadSheetId);
            if (!checkGoogleSpreadsheetService)
            {
                return (false, GoogleSpreadsheetServiceName);
            }

            return (true, "");
        }
    }
}