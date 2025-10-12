using System;
using System.IO;

namespace BillInsight.Helpers
{
    public enum AppFolders
    {
        AppFolder, TempData
    }
        
    public class FolderHelpers
    {
        private static readonly string LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string AppFolder = Path.Combine(LocalApplicationData,  "BillInsight");

        public static string GetFolder(AppFolders folder)
        {
            string folderPath = folder switch
            {
                AppFolders.AppFolder => Path.Combine(LocalApplicationData, "BillInsight"),
                AppFolders.TempData => Path.Combine(AppFolder, "TempData"),
                
                _ => ""
            };

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        public static void CleanTempFolder()
        {
            var filePaths = Directory.GetFiles(GetFolder(AppFolders.TempData));
            if (filePaths.Length == 0)
            {
                return;
            }

            foreach (var filePath in filePaths)
            {
                File.Delete(filePath);
            }
        }
    }
}