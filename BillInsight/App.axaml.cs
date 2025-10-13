using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using BillInsight.Bootstraper;
using BillInsight.ViewModels;
using BillInsight.Views;

namespace BillInsight;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Đăng ký Global Exception Handlers
        RegisterGlobalExceptionHandlers();
        _ = new AppBootstrapper();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private void RegisterGlobalExceptionHandlers()
    {
        // appDomain-level (ngoại lệ không được xử lý trong toàn bộ app)
        AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
        {
            LogException("AppDomain", e.ExceptionObject as Exception);
        };

        // TaskScheduler-level (Task không có await)
        TaskScheduler.UnobservedTaskException += (sender, e) =>
        {
            LogException("TaskScheduler", e.Exception);
            e.SetObserved();
        };

        // UI Thread-level (Avalonia Dispatcher)
        Dispatcher.UIThread.UnhandledException += (sender, e) =>
        {
            LogException("Dispatcher", e.Exception);
            e.Handled = true; // Ngăn app crash
        };
    }

    private void LogException(string source, Exception? ex)
    {
        // Ghi log ra file hoặc hệ thống logging (Serilog, NLog, v.v.)
        Console.WriteLine($"[{source}] Unhandled exception: {ex?.Message}\n{ex}");
            
        // Tùy chọn: hiển thị thông báo cho người dùng
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Dispatcher.UIThread.Post(async () =>
            {
                var msgBox = new Window
                {
                    Width = 600,
                    Height = 200,
                    Title = "Unexpected Error",
                    Content = new TextBlock
                    {
                        Text = ex?.Message ?? "Unknown error",
                        TextWrapping = Avalonia.Media.TextWrapping.Wrap
                    },
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Background = Brushes.White,
                    Foreground = Brushes.Black, 
                    Padding = Thickness.Parse("10"),
                    CornerRadius = CornerRadius.Parse("10")
                };
                await msgBox.ShowDialog(desktop.MainWindow);
            });
        }
    }
}