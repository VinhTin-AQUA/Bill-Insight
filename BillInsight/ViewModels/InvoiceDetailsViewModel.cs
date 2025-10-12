using System.Collections.ObjectModel;
using BillInsight.Models.InvoiceDetails;

namespace BillInsight.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        public ObservableCollection<InvoiceDetailsModel> Products { get; set; } = [];

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