using BillInsight.Models.SpreadSheetInfos;
using ReactiveUI;

namespace BillInsight.Models.Configs
{
    public class AppConfig
    {
        public string ServiceAccountCredentialFilePath { get; set; } = string.Empty; // đường dẫn file cấu hình
        public SheetModel WorkingSheet { get; set; } = new();
        public string SpreadSheetUrl { get; set; } = string.Empty;// url của sheet
        public string SpreadSheetId { get; set; } = string.Empty;// id của spreadsheet
    }
}