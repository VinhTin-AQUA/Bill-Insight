namespace BillInsight.Models.SpreadSheetInfos
{
    public class SpreadSheetInfoModel
    {
        public string Link { get; set; } = string.Empty;
    }

    public class AddSheetModel
    {
        public string SheetName { get; set; } = string.Empty;
    }

    public class Sheet
    {
        public string SheetName { get; set; } = string.Empty;
        public string SheetId { get; set; } = string.Empty;
    }
}