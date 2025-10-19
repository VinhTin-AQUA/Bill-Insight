using System.Text.RegularExpressions;

namespace BillInsight.Helpers
{
    public class SpreadSheetHelpers
    {
        public static string? GetSpreadsheetId(string url)
        {
            var match = Regex.Match(url, @"spreadsheets/d/([a-zA-Z0-9-_]+)");
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
    }
}