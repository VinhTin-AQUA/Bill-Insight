using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class SplashScreenViewModel : ViewModelBase
    {
        private string _Message = "Initializing ...";

        public string Message
        {
            get =>  _Message;
            set => this.RaiseAndSetIfChanged(ref _Message, value);
        }
        
        public SplashScreenViewModel()
        {
            
        }
    }
}