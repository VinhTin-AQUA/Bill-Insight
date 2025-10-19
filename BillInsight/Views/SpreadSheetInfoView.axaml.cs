using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BillInsight.ViewModels;

namespace BillInsight.Views
{
    public partial class SpreadSheetInfoView : UserControl
    {
        public SpreadSheetInfoView()
        {
            InitializeComponent();
            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }
        
        private async void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (DataContext is SpreadSheetInfoViewModel vm)
            {
                await vm.GetListSheets();
            }
        }
    }
}