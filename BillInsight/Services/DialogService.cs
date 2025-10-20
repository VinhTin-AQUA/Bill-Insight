using System;
using System.Threading.Tasks;
using BillInsight.Controls;
using BillInsight.Controls.Dialogs;
using DialogHostAvalonia;

namespace BillInsight.Services
{
    public class DialogService
    {
        public const string MainWindowDialogHostId = "MainWindowDialogHost";
        public const string GoogleSpreadsheetConfigWindowDialogHostId = "GoogleSpreadsheetConfigWindowDialogHost";

        public async Task<bool> ShowYesNoDialogAsync(string hostId, string message)
        {
            var content = new YesNoDialog(message);
            var r = await DialogHost.Show(content, hostId);

            if (r == null)
            {
                return false;
            }
            
            _ = bool.TryParse(r.ToString(), out var result);
            return result;
        }

        public async Task ShowLoadingDialogAsync(string hostId)
        {
            var content = new LoadingDialog();
            await DialogHost.Show(content, hostId);
        }
        
        public async Task ShowMessageDialogAsync(string hostId, string title, string message, bool isSuccess)
        {
            var content = new MessageDialog(title, message, isSuccess);
            _ = await DialogHost.Show(content, hostId);
        }
        
        public void CloseDialog(string hostId, object? result = null)
        {
            DialogHost.Close(hostId, result);
        }
        
        public async Task RunWithLoadingAsync(Func<Task> action, string hostId)
        {
            var content = new LoadingDialog();
            _ = DialogHost.Show(content, hostId);
            try
            {
                await action();
            }
            finally
            {
                DialogHost.Close(hostId);
            }
        }
    }
}