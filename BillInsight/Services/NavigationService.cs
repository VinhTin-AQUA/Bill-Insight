using System;
using BillInsight.ViewModels;
using ReactiveUI;

namespace BillInsight.Services
{
    public class NavigationService : ReactiveObject
    {
        public const string StatisticalView = "StatisticalView";
        public const string InvoiceDetailsView = "InvoiceDetailsView";
        public const string InvoiceImagesView = "InvoiceImagesView";
        public const string AddInvoiceView = "AddInvoiceView";
        public const string SpreadSheetInfoView = "SpreadSheetInfoView";
        public const string SettingsView = "SettingsView";
        
        private string currentPageName = string.Empty;
        private ViewModelBase _currentPage;
        
        public ViewModelBase CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage is IDisposable disposable)
                    disposable.Dispose(); 
                this.RaiseAndSetIfChanged(ref _currentPage, value);
            }
        }

        public NavigationService()
        {
            CurrentPage = new StatisticalViewModel();
        }
    
        public void GoBack() {}

        public void NavigateTo(string pageName)
        {
            if (currentPageName == pageName)
            {
                return;
            }
            currentPageName =  pageName;
            
            switch (pageName)
            {
                case StatisticalView:
                    CurrentPage = new StatisticalViewModel();
                    break;  
                case InvoiceDetailsView:
                    CurrentPage = new InvoiceDetailsViewModel();
                    break;  
                case InvoiceImagesView:
                    CurrentPage = new InvoiceImagesViewModel();
                    break;  
                case AddInvoiceView:
                    CurrentPage = new AddInvoiceViewModel();
                    break;  
                case SpreadSheetInfoView:
                    CurrentPage = new SpreadSheetInfoViewModel();
                    break;  
                case SettingsView:
                    CurrentPage = new SettingsViewModel();
                    break;  
            }
        }
    }
}