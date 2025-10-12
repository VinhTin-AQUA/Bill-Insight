using System.IO;
using Avalonia.Media.Imaging;

namespace BillInsight.Helpers
{
    public class FileHelpers
    {
        public static string ToLocalPath(string path)
        {
            return path.Replace("\\", "/").Replace("file://", "").Trim();
        }

        public static Bitmap BytesToBitmap(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return new Bitmap(ms);
            }
        }
        
        private Bitmap? PathTobitmap(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    // Mở lại file mỗi lần (có thể cache nếu cần)
                    using var stream = File.OpenRead(path);
                    return new Bitmap(stream);
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}