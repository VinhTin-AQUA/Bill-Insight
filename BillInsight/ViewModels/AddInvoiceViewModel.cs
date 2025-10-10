using System;
using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.Products;
using ReactiveUI;

namespace BillInsight.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        private string _name;
        private decimal _price;

        public string Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public decimal Price
        {
            get => _price;
            set => this.RaiseAndSetIfChanged(ref _price, value);
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