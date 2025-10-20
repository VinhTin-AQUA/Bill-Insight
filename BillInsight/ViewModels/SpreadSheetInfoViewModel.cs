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
        public ReactiveCommand<Unit, Unit> SaveWorkingSheetCommand { get; set; }
        public ReactiveCommand<SheetModel, Unit> UpdateSheetCommand { get; set; }

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
                await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Success",
                    "Cập nhật thành công.", true);
            });
            
            AddSheetCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (string.IsNullOrWhiteSpace(NewSheetName))
                {
                    return;
                }

                SheetModel? sheet = null;
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    sheet = await GoogleSpreadsheetService.CreateSheet(NewSheetName);
                    if (sheet != null)
                    {
                        Sheets.Add(new()
                        {
                            Title = sheet.Title,
                            IsActive = false,
                            Id = sheet.Id
                        });
                    }
                }, DialogService.MainWindowDialogHostId);

                if (sheet == null)
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Error",
                        "Thêm sheet thất bại.", false);
                }
                else
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Success",
                        "Thêm thành công.", true);
                }
            });
            
            RemoveSheetCommand = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                var result = await DialogService.ShowYesNoDialogAsync(DialogService.MainWindowDialogHostId, "Bạn có muốn xóa");
                if (result == false)
                {
                    Console.WriteLine("False");
                    return;
                }
                
                var item = Sheets.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return;
                }

                if (item.IsActive)
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Error",
                        "Sheet này đang làm việc.", false);
                    return;
                }
                
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    var r = await GoogleSpreadsheetService.RemoveSheet(id);
                    if (!r)
                    {
                        return;
                    }
                    Sheets.Remove(item);
                }, DialogService.MainWindowDialogHostId);
            });
            
            SaveWorkingSheetCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var model = Sheets.FirstOrDefault(x => x.IsActive);
                if (model == null)
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Error",
                        "Không tìm thấy sheet.", false);
                }
                else
                {
                    ConfigService.Config.WorkingSheet.Title = model.Title;
                    ConfigService.Config.WorkingSheet.Id = model.Id;
                    ConfigService.Config.WorkingSheet.IsActive = true;
                    await ConfigService.UpdateConfigAsync();
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Success",
                        "Cập nhật thành công.", true);
                }
            });
            
            UpdateSheetCommand = ReactiveCommand.CreateFromTask<SheetModel>(async (model) =>
            {
                SheetModel? sheet = null;
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    sheet = await GoogleSpreadsheetService.UpdateSheet(model);
                }, DialogService.MainWindowDialogHostId);

                if (sheet == null)
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Error",
                        "Cập nhật sheet thất bại.", false);
                }
                else
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Success",
                        "Cập nhật thành công.", true);
                    ConfigService.Config.WorkingSheet.Title = model.Title;
                    await ConfigService.UpdateConfigAsync();
                }
            });
        }

        public async Task GetListSheets()
        {
            await DialogService.RunWithLoadingAsync(async () =>
            {
                var sheets = await GoogleSpreadsheetService.GetSheets();
                
                var model = sheets.FirstOrDefault(x => x.Id == ConfigService.Config.WorkingSheet.Id);
                if (model != null)
                {
                    model.IsActive = true;
                }
                Sheets.AddRange(sheets);
            }, DialogService.MainWindowDialogHostId);
            
            // Sheets.AddRange([
            // new()
            // {
            //     IsActive = false,
            //     Title = "New Sheet",
            // }]);
        }
    }
}