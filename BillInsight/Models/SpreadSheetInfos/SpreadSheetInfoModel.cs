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

    public class SheetModel
    {
        public int? Id { get; set; } = -1;
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}