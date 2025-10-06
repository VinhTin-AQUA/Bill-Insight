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
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.StatisticalView);
                })
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/excel.svg",
                Name = "Chi tiết",
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.InvoiceDetailsView);
                })
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/image.svg",
                Name = "Ảnh hóa đơn",
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.InvoiceImagesView);
                })
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/edit.svg",
                Name = "Thêm hóa đơn",
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.AddInvoiceView);
                })
            },
            
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/info.svg",
                Name = "Thông tin Sheet",
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.SpreadSheetInfoView);
                })
            },
            new()
            {
                Icon = "avares://BillInsight/Assets/SVGs/settings.svg",
                Name = "Settings",
                NavigateCommand = ReactiveCommand.Create(() =>
                {
                    NavigationService.NavigateTo(NavigationService.SettingsView);
                })
            },
        ];

        public ReactiveCommand<SidebarItem, Unit> NavigateCommand { get; set; }
        
        public SidebarViewModel()
        {
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}