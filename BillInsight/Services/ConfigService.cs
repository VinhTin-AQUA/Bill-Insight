using System;
using System.IO;
using System.Threading.Tasks;
using BillInsight.Helpers;
using BillInsight.Models.Configs;
using JsonFlatFileDataStore;

namespace BillInsight.Services
{
    public class ConfigService
    {
        private const string ConfigJsonFileName = "config.json";
        private const string AppConfigKey = "AppConfig";
        private DataStore store;
        
        public AppConfig? Config { get; set; }

        public ConfigService()
        {
            var configFolder = FolderHelpers.GetFolder(AppFolders.Config);
            var configJsonFilePath = Path.Combine(configFolder, ConfigJsonFileName);
            store = new DataStore(configJsonFilePath);
        }

        public async Task InitAsync()
        {
            try
            {
                Config = store.GetItem<AppConfig>(AppConfigKey);
            }
            catch
            {
                Config = new AppConfig();
                _ = await store.InsertItemAsync(AppConfigKey, Config);
            }
        }

        public async Task UpdateConfigAsync()
        {
             await store.ReplaceItemAsync(AppConfigKey, Config);
        }
    }
}