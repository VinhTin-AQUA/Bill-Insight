using System.Collections.ObjectModel;
using BillInsight.Models.Sidebar;
using BillInsight.Services;
using Splat;

namespace BillInsight.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public NavigationService NavigationService { get; set; }

        public ObservableCollection<SidebarItem> Items { get; set; } = [
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/menu.svg",
                Name = "Converter"
            }];

        public SidebarViewModel()
        {
            this.NavigationService = Locator.Current.GetService<NavigationService>()!;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}