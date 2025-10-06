namespace BillInsight.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    public override void Dispose()
    {
        // các lệnh giải phóng tài nguyên
        base.Dispose();
    }
}