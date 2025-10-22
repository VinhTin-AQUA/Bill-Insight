using System.Threading.Tasks;
using BillInsight.Services;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class StatisticalViewModel : ViewModelBase
    {
        private string _totalCashUsed = "0đ"; // Tổng tiền mặt đã dùng
        public string TotalCashUsed
        {
            get => _totalCashUsed;
            set => this.RaiseAndSetIfChanged(ref _totalCashUsed, value);
        }

        private string _totalBankUsed = "0đ"; // Tổng ngân hàng đã dùng
        public string TotalBankUsed
        {
            get => _totalBankUsed;
            set => this.RaiseAndSetIfChanged(ref _totalBankUsed, value);
        }

        private string _monthlyCashTotal = "0đ"; // Tổng tiền mặt mỗi tháng
        public string MonthlyCashTotal
        {
            get => _monthlyCashTotal;
            set => this.RaiseAndSetIfChanged(ref _monthlyCashTotal, value);
        }

        private string _monthlyBankTotal = "0đ"; // Tổng tiền ngân hàng mỗi tháng
        public string MonthlyBankTotal
        {
            get => _monthlyBankTotal;
            set => this.RaiseAndSetIfChanged(ref _monthlyBankTotal, value);
        }

        private string _remainingCash = "0đ"; // Tiền mặt còn lại
        public string RemainingCash
        {
            get => _remainingCash;
            set => this.RaiseAndSetIfChanged(ref _remainingCash, value);
        }

        private string _remainingBank = "0đ"; // Tiền ngân hàng còn lại
        public string RemainingBank
        {
            get => _remainingBank;
            set => this.RaiseAndSetIfChanged(ref _remainingBank, value);
        }

        private string _totalRemaining = "0đ"; // Tổng tiền còn lại
        public string TotalRemaining
        {
            get => _totalRemaining;
            set => this.RaiseAndSetIfChanged(ref _totalRemaining, value);
        }
        
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; set; }
        public ConfigService ConfigService { get; set; }
        public DialogService DialogService { get; set; }
        
        public StatisticalViewModel()
        {
            GoogleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;
            ConfigService = Locator.Current.GetService<ConfigService>()!;
            DialogService = Locator.Current.GetService<DialogService>()!;
        }

        public async Task GetStatistics()
        {
            await DialogService.RunWithLoadingAsync(async () =>
            {
                var values =
                    await GoogleSpreadsheetService.GetDataRow<string>(ConfigService.Config.WorkingSheet.Title, "E2:K2");
                TotalCashUsed = values[0];
                TotalBankUsed = values[1];
                MonthlyCashTotal = values[2];
                MonthlyBankTotal = values[3];
                RemainingCash = values[4];
                RemainingBank = values[5];
                TotalRemaining = values[6];
            }, DialogService.MainWindowDialogHostId);
        }
        
        public virtual void Dispose()
        {
            // override trong ViewModel con để giải phóng resource
        }
    }
}