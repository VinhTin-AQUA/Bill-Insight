using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace BillInsight.Services
{
    public class WindowNavigationService
    {
        public void ShowWindow<TWindow>() where TWindow : Window, new()
        {
            var window = new TWindow();
            
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = window;
            }
            window.Show();
        }
        
        public void ShowWindow<TWindow, TViewModel>()
            where TWindow : Window, new()
            where TViewModel : class, new()
        {
            var vm = new TViewModel();
            var window = new TWindow
            {
                DataContext = vm
            };

            window.Show();
        }

        public void CloseCurrentWindow(Window window)
        {
            window.Close();
        }
    }
}