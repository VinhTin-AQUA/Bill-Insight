using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using BillInsight.Helpers;
using BillInsight.ViewModels;

namespace BillInsight.Views
{
    public partial class AddInvoiceView : UserControl
    {
        public AddInvoiceView()
        {
            InitializeComponent();
        }
        
        private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
        {
            // // Get top level from the current control. Alternatively, you can use Window reference instead.
            // var topLevel = TopLevel.GetTopLevel(this);
            //
            // // Start async operation to open the dialog.
            // var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            // {
            //     Title = "Open Text File",
            //     AllowMultiple = false,
            //     FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            // });
            //
            // if (files.Count >= 1)
            // {
            //     var file = files[0];
            //     
            //     if (DataContext is AddInvoiceViewModel vm)
            //     {
            //         vm.FilePath = FileHelpers.ToLocalPath(file.Path.ToString());
            //     }
            // }
        }
    }
}