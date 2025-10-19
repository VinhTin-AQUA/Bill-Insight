using System;
using System.Reactive;
using System.Threading.Tasks;
using BillInsight.Services;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class GoogleSpreadsheetConfigWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; }
        
        public event Action? ConfigCompleted;
        
        public ConfigService ConfigService { get; set; }
        public DialogService DialogService { get; set; }
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; set; }
        
        public GoogleSpreadsheetConfigWindowViewModel()
        {
            ConfigService = Locator.Current.GetService<ConfigService>()!;
            DialogService = Locator.Current.GetService<DialogService>()!;
            GoogleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;
            
            SaveConfigCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    await ConfigService.UpdateConfigAsync();
                    GoogleSpreadsheetService.Init(ConfigService.Config.ServiceAccountCredentialFilePath, ConfigService.Config.SpreadSheetId);
                }, DialogService.GoogleSpreadsheetConfigWindowDialogHostId);
                
                ConfigCompleted?.Invoke();
            });
        }
    }
}