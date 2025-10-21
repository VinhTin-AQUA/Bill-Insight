using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BillInsight.Models.InvoiceDetails;
using BillInsight.Services;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        private ObservableCollection<InvoiceDetails> _invoiceDetails = new();
        public ObservableCollection<InvoiceDetails> InvoiceDetails
        {
            get => _invoiceDetails;
            set => this.RaiseAndSetIfChanged(ref _invoiceDetails, value);
        }
        
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; set; }
        public ConfigService ConfigService { get; set; }

        public InvoiceDetailsViewModel()
        { 
            GoogleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;
            ConfigService = Locator.Current.GetService<ConfigService>()!;
        }

        public async Task GetInvoiceDetails()
        {
            var values =
                await GoogleSpreadsheetService.GetDataMatrix<string>(ConfigService.Config.WorkingSheet.Title, "A:D", true);
            
            string currentDate;

            foreach (var item in values)
            {
                // Nếu item có Date khác "-", cập nhật ngày hiện tại
                if (item[0] != "-")
                {
                    currentDate = item[0];
                    List<InvoiceItem> temps = [
                        new()
                        {
                            ItemName = item[1],
                            Cash = item[2],
                            Bank = item[3]
                        }
                    ];
                    
                    InvoiceDetails.Add(new InvoiceDetails
                    {
                        DatePurchased = currentDate,
                        Items = new List<InvoiceItem>(temps)
                    });
                }
                else
                {
                    // Nếu là "-", thêm vào Invoice cuối cùng
                    var lastInvoice = InvoiceDetails[^1];
                    InvoiceItem temp = new()
                    {
                        ItemName = item[1],
                        Cash = item[2],
                        Bank = item[3]
                    };
                    lastInvoice.Items.Add(temp);
                }
            }
        }
    }
}