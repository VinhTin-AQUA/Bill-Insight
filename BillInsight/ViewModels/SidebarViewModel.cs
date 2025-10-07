using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.Sidebar;
using BillInsight.Services;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class SidebarViewModel : ViewModelBase
    {
        public static NavigationService NavigationService { get; set; } = Locator.Current.GetService<NavigationService>()!;

        public ObservableCollection<SidebarItem> Items { get; set; } = [
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/chart.svg",
                Name = "Thống kê",
                RouteName = NavigationService.StatisticalView,
                IsActive = true
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/excel.svg",
                Name = "Chi tiết",
                RouteName = NavigationService.InvoiceDetailsView,
                IsActive = false,
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/image.svg",
                Name = "Ảnh hóa đơn",
                RouteName = NavigationService.InvoiceImagesView,
                IsActive = false,
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/edit.svg",
                Name = "Thêm hóa đơn",
                RouteName = NavigationService.AddInvoiceView,
                IsActive = false,
            },
            
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/info.svg",
                Name = "Thông tin Sheet",
                RouteName = NavigationService.SpreadSheetInfoView,
                IsActive = false,
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/settings.svg",
                Name = "Settings",
                RouteName = NavigationService.SettingsView,
                IsActive = false,
            },
        ];

        private SidebarItem CurrentPage { get; set; }
        
        public ReactiveCommand<SidebarItem, Unit> NavigateCommand { get; set; }

        public SidebarViewModel()
        {
            CurrentPage = Items[0];
            NavigateCommand = ReactiveCommand.Create<SidebarItem>((item) =>
            {
                CurrentPage.IsActive = false;
                item.IsActive = true;
                CurrentPage = item;
                NavigationService.NavigateTo(item.RouteName);
            });
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}