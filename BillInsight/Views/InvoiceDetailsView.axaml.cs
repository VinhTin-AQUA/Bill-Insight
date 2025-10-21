using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BillInsight.ViewModels;

namespace BillInsight.Views
{
    public partial class InvoiceDetailsView : UserControl
    {
        public InvoiceDetailsView()
        {
            InitializeComponent();
            this.AttachedToVisualTree += OnAttachedToVisualTree;
        }
        
        private async void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            if (DataContext is InvoiceDetailsViewModel vm)
            {
                await vm.GetInvoiceDetails();
            }
        }
    }
}