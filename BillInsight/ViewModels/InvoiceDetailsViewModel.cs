using System;
using System.Collections.ObjectModel;
using BillInsight.Models.Products;

namespace BillInsight.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<Invoice> Products { get; set; } = 
        [
            
        ];

        public InvoiceDetailsViewModel()
        {
        }
        
        public override void Dispose()
        {
            // các lệnh giải phóng tài nguyên
            base.Dispose();
        }
    }
}