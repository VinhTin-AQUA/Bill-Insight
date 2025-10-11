namespace BillInsight.Helpers
{
    public class FileHelpers
    {
        public static string ToLocalPath(string path)
        {
            return path.Replace("\\", "/").Replace("file://", "").Trim();
        }
    }
}