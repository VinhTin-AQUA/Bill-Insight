using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;

namespace BillInsight.Converters
{
    public class ImagePathToBitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            try
            {
                if (value is string path && !string.IsNullOrWhiteSpace(path))
                {
                    // Trường hợp path là file cục bộ
                    if (File.Exists(path))
                        return new Bitmap(path);

                    using var stream = File.OpenRead(path);
                    return new Bitmap(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ImagePathToBitmapConverter] Error: {ex.Message}");
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}