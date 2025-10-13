using System;
using System.Globalization;

namespace BillInsight.Helpers
{
    public class NumberHelpers
    {
        public static decimal ParsePercentage(string percentageString)
        {
            // Loại bỏ khoảng trắng và ký tự % (nếu có)
            string cleanString = percentageString.Trim().Replace("%", "");
    
            // Sử dụng NumberStyles để cho phép định dạng phần trăm
            if (decimal.TryParse(cleanString, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result / 100m;
            }
    
            throw new FormatException($"Không thể chuyển đổi '{percentageString}' thành số phần trăm");
        }
    }
}