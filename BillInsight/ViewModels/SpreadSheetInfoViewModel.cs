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
        
        public ReactiveCommand<Unit, Unit> SaveConfigCommand { get; set; }
        public ReactiveCommand<Unit, Unit> RemoveSheetCommand { get; set; }

        public string NewSheetName { get; set; } = string.Empty;
        
        #region services

        private YesNoDialogService YesNoDialogService { get; set; }
        public ConfigService ConfigService { get; set; }

        #endregion

        public SpreadSheetInfoViewModel()
        {
            YesNoDialogService = Locator.Current.GetService<YesNoDialogService>()!;
            ConfigService = Locator.Current.GetService<ConfigService>()!;

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
            SaveConfigCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await ConfigService.UpdateConfigAsync();
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