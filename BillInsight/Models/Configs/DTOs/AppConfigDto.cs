using BillInsight.Models.SpreadSheetInfos;

namespace BillInsight.Models.Configs.DTOs
{
    public class AppConfigDto
    {
        public string ServiceAccountCredentialFilePath { get; set; } = string.Empty; // đường dẫn file cấu hình
        public SheetModel WorkingSheet { get; set; } = new();
        public string SpreadSheetUrl { get; set; } = string.Empty;// url của sheet

        public string SpreadSheetId { get; set; } = string.Empty;
    }
}