using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using BillInsight.Models.SpreadSheetInfos;
using BillInsight.Services;
using DynamicData;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class SpreadSheetInfoViewModel : ViewModelBase
    {
        private ObservableCollection<SheetModel> _sheets = new();
        public ObservableCollection<SheetModel> Sheets
        {
            get => _sheets;
            set => this.RaiseAndSetIfChanged(ref _sheets, value);
        }
        
        public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddSheetCommand { get; set; }
        public ReactiveCommand<int, Unit> RemoveSheetCommand { get; set; }

        public string NewSheetName { get; set; } = string.Empty;
        
        #region services

        private DialogService DialogService { get; set; }
        public ConfigService ConfigService { get; set; }
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; set; }

        #endregion

        public SpreadSheetInfoViewModel()
        {
            DialogService = Locator.Current.GetService<DialogService>()!;
            ConfigService = Locator.Current.GetService<ConfigService>()!;
            GoogleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;

            InitCommands();
        }

        private void InitCommands()
        {
            SaveConfigCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await ConfigService.UpdateConfigAsync();
            });
            
            AddSheetCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (string.IsNullOrWhiteSpace(NewSheetName))
                {
                    return;
                }

                await DialogService.RunWithLoadingAsync(async () =>
                {
                    var r = await GoogleSpreadsheetService.CreateSheet(NewSheetName);
                    if (r)
                    {
                        Sheets.Add(new()
                        {
                            Title = NewSheetName,
                            IsActive = false,
                        });
                    }
                }, DialogService.MainWindowDialogHostId);
            });
            
            RemoveSheetCommand = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                var result = await DialogService.ShowYesNoDialogAsync(DialogService.MainWindowDialogHostId);
                if (result == false)
                {
                    Console.WriteLine("False");
                    return;
                }

                await DialogService.RunWithLoadingAsync(async () =>
                {
                    var r = await GoogleSpreadsheetService.RemoveSheet(id);
                    if (!r)
                    {
                        return;
                    }
                    var item = Sheets.FirstOrDefault(x => x.Id == id);
                    if (item == null)
                    {
                        return;
                    }
                    Sheets.Remove(item);
                }, DialogService.MainWindowDialogHostId);
            });
        }

        public async Task GetListSheets()
        {
            // var sheets = await GoogleSpreadsheetService.GetSheets();
            // Sheets.AddRange(sheets);
            Sheets.AddRange([
            new()
            {
                IsActive = false,
                Title = "New Sheet",
            }]);
        }
    }
}