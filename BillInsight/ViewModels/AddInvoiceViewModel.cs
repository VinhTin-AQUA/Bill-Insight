using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using Avalonia.Media.Imaging;
using BillInsight.Models.Products;
using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _filePath, value); 
                LoadImage(value);
            }
        }
        
        private Bitmap? _imageSource;
        public Bitmap? ImageSource
        {
            get => _imageSource;
            set => this.RaiseAndSetIfChanged(ref _imageSource, value);
        }

        public ReactiveCommand<Unit, Unit> RemoveImageCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddProductCommand { get; set; }
        public ReactiveCommand<string, Unit> RemoveProductCommand { get; set; }
        
        public ObservableCollection<ProductModel> Products { get; set; } =
        [
            new("Sản phẩm A", 10000, "A"),
            new("Sản phẩm B", 15000, "B"),
            new("Sản phẩm C", 20000, "C" )
        ];
        
        public AddInvoiceViewModel()
        {
            RemoveImageCommand = ReactiveCommand.Create(() =>
            {
                ImageSource = null;
                FilePath = "";
            });
            
            AddProductCommand = ReactiveCommand.Create(() =>
            {
                Products.Add(new("", 0, ""));
            });
            
            RemoveProductCommand = ReactiveCommand.Create<string>((id) =>
            {
                var item = Products.FirstOrDefault(x => x.Id == id);
                if (item != null)
                {
                    Products.Remove(item);
                }
            });
        }
        
        private void LoadImage(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    // Mở lại file mỗi lần (có thể cache nếu cần)
                    using var stream = File.OpenRead(path);
                    ImageSource = new Bitmap(stream);
                }
                catch
                {
                    ImageSource = null;
                }
            }
            else
            {
                ImageSource = null;
            }
        }
        
        public override void Dispose()
        {
            // các lệnh giải phóng tài nguyên
            base.Dispose();
        }
    }
}