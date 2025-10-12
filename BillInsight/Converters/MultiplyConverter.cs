using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;

namespace BillInsight.Converters
{
    public class MultiplyConverter : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            if (values.Count >= 2 &&
                decimal.TryParse(values[0]?.ToString(), out decimal a) &&
                decimal.TryParse(values[1]?.ToString(), out decimal b))
            {
                return a + a * b;
            }

            return 0;
        }
    }
}