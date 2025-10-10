using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;

namespace BillInsight.Views
{
    public partial class AddInvoiceView : UserControl
    {
        public AddInvoiceView()
        {
            InitializeComponent();
        }
        
        private async void OnChooseImageClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Filters =
                {
                    new FileDialogFilter() { Name = "Ảnh", Extensions = { "png", "jpg", "jpeg", "bmp" } }
                }
            };

            string[]? result = await dialog.ShowAsync(this);

            if (result != null && result.Length > 0)
            {
                string filePath = result[0];

                // Mở file ảnh và gán vào Image control
                using (var stream = File.OpenRead(filePath))
                {
                    PreviewImage.Source = new Bitmap(stream);
                }
            }
        }
    }
}