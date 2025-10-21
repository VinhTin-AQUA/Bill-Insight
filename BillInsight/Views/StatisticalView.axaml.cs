using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BillInsight.ViewModels;

namespace BillInsight.Views
{
    public partial class StatisticalView : UserControl
    {
        public StatisticalView()
        {
            InitializeComponent();
            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }
        
        private async void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (DataContext is StatisticalViewModel vm)
            {
                await vm.GetStatistics();
            }
        }
    }
}