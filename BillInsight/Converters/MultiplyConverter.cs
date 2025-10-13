using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using BillInsight.Helpers;

namespace BillInsight.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            return "0";
            if (values.Count == 0)
            {
                return "0";
            }
            
            string? valueString = values[1]?.ToString();
            if (valueString == null)
            {
                return "0";
            }
            decimal b = NumberHelpers.ParsePercentage(valueString);
            if (decimal.TryParse(values[0]?.ToString(), out decimal a))
            {
                return (a + a * b).ToString(CultureInfo.InvariantCulture);
            }
            return "0";
        }
    }
}