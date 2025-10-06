using BillInsight.Services;
using Splat;

namespace BillInsight.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    public NavigationService NavigationService { get; set; }
    
    public MainWindowViewModel()
    {
        this.NavigationService = NavigationService ??  Locator.Current.GetService<NavigationService>()!;
    }
    
    public override void Dispose()
    {
        // các lệnh giải phóng tài nguyên
        base.Dispose();
    }
}