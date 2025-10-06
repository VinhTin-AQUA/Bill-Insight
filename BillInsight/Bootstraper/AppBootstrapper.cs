using BillInsight.Services;
using Splat;

namespace BillInsight.Bootstraper
{
    public class AppBootstrapper : IEnableLogger
    {
        public AppBootstrapper()
        {
            Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(NavigationService));
        }
    }
}