namespace BillInsight.Models.Statisticals
{
    public class StatisticalModel
    {
        public string TotalMonthlyWear { get; set; } = string.Empty; // Tổng tiền mặc mỗi tháng
        public string TotalMonthlyBankMoney { get; set; } = string.Empty; // Tổng tiền ngân hàng mỗi tháng
        public string TotalCashUsed { get; set; } = string.Empty; // Tổng tiền mặt đã dùng
        public string TotalBankMoneyUsed { get; set; } = string.Empty; //Tổng tiền ngân hàng đã dùng
        public string RemainingWear { get; set; } = string.Empty; // Tiền mặc còn lại
        public string RemainingBankMoney { get; set; } = string.Empty; // Tiền ngân hàng còn lại
        public string TotalRemaining { get; set; } = string.Empty; // Tổng tiền còn lại
    }
}