using System;
using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.Configs;
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
        
        public ReactiveCommand<Unit, Unit> SaveConfigSheetCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveSheetCommand { get; set; }

        public AppConfig AppConfig { get; set; }
        public string NewSheetName { get; set; } = string.Empty;
        
        #region services

        private YesNoDialogService YesNoDialogService { get; set; }

        #endregion

        public SpreadSheetInfoViewModel()
        {
            YesNoDialogService = Locator.Current.GetService<YesNoDialogService>()!;
            Sheets.AddRange([
                new SheetModel() { Id = 1, Title = "Sheet1", IsActive = false},
                new SheetModel() { Id = 2, Title = "Sheet2", IsActive = false },
                new SheetModel() { Id = 3, Title = "Sheet3", IsActive = true },
                new SheetModel() { Id = 4, Title = "Sheet4", IsActive = false },
                new SheetModel() { Id = 5, Title = "Sheet5", IsActive = false },
            ]);

            InitCommands();
        }

        private void InitCommands()
        {
            SaveConfigSheetCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                // kiểm tra file cấu hình có chưa
                // chưa có thì thêm mới
                // có rồi thì load file cấu hình, nếu load không được thì xóa file và tạo lại với dữ liệu trống
                
            });
            
            RemoveSheetCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var result = await YesNoDialogService.ShowDialogAsync();
                if (result is string text)
                {
                    Console.WriteLine($"Người dùng nhập: {text}");
                }
            });
        }
    }
}