using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using Avalonia.Media.Imaging;
using BillInsight.Models.Products;
using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value); 
        }
        
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

        
        public ObservableCollection<ProductModel> Products { get; set; } =
        [
            new ProductModel() { Name = "Sản phẩm A", Price = 10000, PaymentMethod = "A" },
            new ProductModel() { Name = "Sản phẩm B", Price = 15000, PaymentMethod = "B" },
            new ProductModel() { Name = "Sản phẩm C", Price = 20000, PaymentMethod = "C" }
        ];
        
        
        public AddInvoiceViewModel()
        {
           
        }
        
        public override void Dispose()
        {
            // các lệnh giải phóng tài nguyên
            base.Dispose();
        }
    }
}