using System.Threading.Tasks;
using BillInsight.Controls;
using DialogHostAvalonia;

namespace BillInsight.Services
{
    public class YesNoDialogService
    {
        private const string HostId = "GlobalDialogHost";

        public async Task<object?> ShowDialogAsync()
        {
            var content = new YesNoDialog();
            var r = await DialogHost.Show(content, HostId);
            return r;
        }

        public void CloseDialog(object? result = null)
        {
            DialogHost.Close(HostId, result);
        }
    }
}