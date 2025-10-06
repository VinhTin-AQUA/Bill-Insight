using System;
using BillInsight.ViewModels;
using ReactiveUI;

namespace BillInsight.Services
{
    public class NavigationService : ReactiveObject
    {
        public const string HomeName = "Statistical";
        
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
            switch (pageName)
            {
                case HomeName:
                    CurrentPage = new StatisticalViewModel();
                    break;  
            }
        }
    }
}