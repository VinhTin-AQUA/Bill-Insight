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
        private static int Index = -1;
        
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;

        public SheetModel()
        {
            Id = Index;
            Index--;
        }
    }
}