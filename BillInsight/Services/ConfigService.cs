using System.IO;
using System.Threading.Tasks;
using BillInsight.Helpers;
using BillInsight.Models.Configs;
using BillInsight.Models.Configs.DTOs;
using JsonFlatFileDataStore;

namespace BillInsight.Services
{
    public class ConfigService
    {
        private const string ConfigJsonFileName = "config.json";
        private const string AppConfigKey = "AppConfig";
        private DataStore store;

        public AppConfig Config { get; set; } = new();

        public ConfigService()
        {
            var configFolder = FolderHelpers.GetFolder(AppFolders.Config);
            var configJsonFilePath = Path.Combine(configFolder, ConfigJsonFileName);
            store = new DataStore(configJsonFilePath);
        }

        public async Task<bool> InitAsync()
        {
            try
            {
                var appConfigDto = store.GetItem<AppConfigDto>(AppConfigKey);
                Config = new()
                {
                    ServiceAccountCredentialFilePath = appConfigDto.ServiceAccountCredentialFilePath,
                    SpreadSheetUrl = appConfigDto.SpreadSheetUrl,
                    SpreadSheetId = appConfigDto.SpreadSheetId,
                    WorkingSheet = new()
                    {
                        Id = appConfigDto.WorkingSheet.Id,
                        Title = appConfigDto.WorkingSheet.Title,
                        IsActive = appConfigDto.WorkingSheet.IsActive
                    }
                };
            }
            catch
            {
                var appConfigDto = new AppConfigDto();
                _ = await store.InsertItemAsync(AppConfigKey, appConfigDto);
            }
            return true;
        }

        public async Task UpdateConfigAsync()
        {
            var configData = new
            {
                Config.ServiceAccountCredentialFilePath,
                Config.SpreadSheetUrl,
                WorkingSheet = new
                {
                    Config.WorkingSheet.Id,
                    Config.WorkingSheet.Title,
                    Config.WorkingSheet.IsActive,
                },
                Config.SpreadSheetId
            };
            await store.ReplaceItemAsync(AppConfigKey, configData);
        }
    }
}