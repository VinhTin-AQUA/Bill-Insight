using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BillInsight.Helpers;
using BillInsight.Services;
using BillInsight.ViewModels;
using Splat;

namespace BillInsight.Views
{
    public partial class GoogleSpreadsheetConfigWindow : Window
    {
        public GoogleSpreadsheetConfigWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }
        
        private void OnDataContextChanged(object? sender, EventArgs e)
        {
            if (DataContext is GoogleSpreadsheetConfigWindowViewModel vm)
            {
                var windowNavigationService = Locator.Current.GetService<WindowNavigationService>()!;
                vm.ConfigCompleted += () =>
                {
                    windowNavigationService.ShowWindow<MainWindow, MainWindowViewModel>();
                    Close();
                };
            }
        }

        private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
        {
            if (DataContext is GoogleSpreadsheetConfigWindowViewModel vm)
            {
                vm.ConfigService.Config.SpreadSheetId = SpreadSheetHelpers.GetSpreadsheetId(vm.ConfigService!.Config!.SpreadSheetUrl) ?? "";
            }
        }
    }
}