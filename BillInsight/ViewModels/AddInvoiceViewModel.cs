using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Models.AddInvoices;
using BillInsight.Services;
using ReactiveUI;
using Splat;
using System.Linq;
using BillInsight.Helpers;

namespace BillInsight.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        #region props
        
        private NBan _NBan = new();
        public NBan NBan
        {
            get => _NBan;
            set => this.RaiseAndSetIfChanged(ref _NBan, value);
        }
        
        // public ObservableCollection<HHDVu> HHDVuList { get; set; } = [];
        private ObservableCollection<HHDVu> _hhdVuList = new();
        public ObservableCollection<HHDVu> HHDVuList
        {
            get => _hhdVuList;
            set => this.RaiseAndSetIfChanged(ref _hhdVuList, value);
        }
        
        private ObservableCollection<HHDVu> _hhdVuByCash= new();
        public ObservableCollection<HHDVu> HHDVuByCash
        {
            get => _hhdVuByCash;
            set => this.RaiseAndSetIfChanged(ref _hhdVuByCash, value);
        }
        
        private TToan _TToan = new();
        public TToan TToan
        {
            get => _TToan;
            set => this.RaiseAndSetIfChanged(ref _TToan, value);
        }
        
        private InvoiceModel invoiceModel = new();
        public InvoiceModel InvoiceModel
        {
            get => invoiceModel;
            set => this.RaiseAndSetIfChanged(ref invoiceModel, value);
        }

        private DateTime _invoiceDate = DateTime.Now;
        public DateTime InvoiceDate
        {
            get => _invoiceDate;
            set => this.RaiseAndSetIfChanged(ref _invoiceDate, value);
        }
        
        #endregion

        #region commands

        public ReactiveCommand<Unit, Unit> RemoveImageCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GetCaptchaCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SendAPICommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveInvoiceCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddInvoiceDetailCommand { get; set; }
        public ReactiveCommand<Unit, Unit> AddInvoiceByCashDetailCommand { get; set; }
        public ReactiveCommand<string, Unit> RemoveInvoiceDetailCommand { get; set; }
        public ReactiveCommand<string, Unit> RemoveInvoiceByCashDetailCommand { get; set; }

        #endregion
        
        #region 
        
        public CookieModel? cookieModel { get; set; }
        
        #endregion

        #region servives

        public BachHoaXanhService BachHoaXanhService { get; set; }
        public GoogleSpreadsheetService GoogleSpreadsheetService { get; set; }
        public ConfigService ConfigService { get; set; }
        public DialogService DialogService { get; set; }
        
        #endregion
        
        public AddInvoiceViewModel()
        {
            BachHoaXanhService = Locator.Current.GetService<BachHoaXanhService>()!;
            GoogleSpreadsheetService = Locator.Current.GetService<GoogleSpreadsheetService>()!;
            ConfigService = Locator.Current.GetService<ConfigService>()!;
            DialogService = Locator.Current.GetService<DialogService>()!;
            InitCommands();
        }
        
        private void InitCommands()
        {
            GetCaptchaCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    cookieModel = await BachHoaXanhService.GetCaptchaAndASPSession();
                    if (cookieModel != null)
                    {
                        InvoiceModel.CaptchaPath = cookieModel.CaptchaPath;
                    }
                }, DialogService.MainWindowDialogHostId);
            });

            SendAPICommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (cookieModel == null)
                {
                    return;
                }

                await DialogService.RunWithLoadingAsync(async () =>
                {
                    var (_, _, urlTaiXML) = await BachHoaXanhService.SendAPI(cookieModel.SvID, 
                        cookieModel.ASPNET_SessionId, 
                        InvoiceModel.CaptchaCode, 
                        InvoiceModel.MaCT, 
                        InvoiceModel.MaHD
                    );
                
                    var xmlPath = await BachHoaXanhService.DownloadXMLFile(urlTaiXML);
                    // string xmlPath = "/home/newtun/.local/share/BillInsight/TempData/temp.xml";
                
                    (NBan nban, List<HHDVu> hhdvuList, TToan ttoan) = await BachHoaXanhService.GetXAMLData(xmlPath);
                    NBan =  nban;
                    HHDVuList = new ObservableCollection<HHDVu>(hhdvuList);
                    TToan =  ttoan;
                }, DialogService.MainWindowDialogHostId);
            });

            AddInvoiceDetailCommand = ReactiveCommand.Create(() =>
            {
                HHDVuList.Add(new HHDVu());
            });
            
            AddInvoiceByCashDetailCommand = ReactiveCommand.Create(() =>
            {
                HHDVuByCash.Add(new HHDVu());
            });

            RemoveInvoiceDetailCommand = ReactiveCommand.Create<string>((id) =>
            {
                var item = HHDVuList.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return;
                }
                HHDVuList.Remove(item);
            });
            
            RemoveInvoiceByCashDetailCommand = ReactiveCommand.Create<string>((id) =>
            {
                var item = HHDVuByCash.FirstOrDefault(x => x.Id == id);
                if (item == null)
                {
                    return;
                }
                HHDVuByCash.Remove(item);
            });
                
            SaveInvoiceCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var totalItems = HHDVuList.Concat(HHDVuByCash).ToList();
                int total = totalItems.Count;
                
                if (total == 0)
                {
                    await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Notice", "There are no invoices to add.", false);
                    return;
                }
                
                List<IList<object>> values = [];
                values.Add(new List<object>() 
                    {
                        InvoiceDate.ToString("dd/MM/yyyy"), 
                        totalItems[0].THHDVu.Trim(), 
                        totalItems[0].Cash.Trim() == "" ? "0" : totalItems[0].Cash.Trim(), 
                        totalItems[0].ThTienSauLaiSuat == 0 ? "0" : totalItems[0].ThTienSauLaiSuat
                    }
                );
                
                for (int i = 1; i < total; i++)
                {
                    string date = "-";
                    string itemName = totalItems[i].THHDVu.Trim();
                    string cash  = totalItems[i].Cash.Trim() == "" ? "0" : totalItems[i].Cash.Trim();
                    var bank = totalItems[i].ThTienSauLaiSuat == 0 ? "0" : $"{totalItems[i].ThTienSauLaiSuat}";
                    values.Add(new List<object> { date, itemName, cash, bank });
                }
                
                
                await DialogService.RunWithLoadingAsync(async () =>
                {
                    var r = await GoogleSpreadsheetService.Append(ConfigService.Config.WorkingSheet.Title, "A2", values);
                }, DialogService.MainWindowDialogHostId);
                
                FolderHelpers.CleanTempFolder();
                await DialogService.ShowMessageDialogAsync(DialogService.MainWindowDialogHostId, "Success", "Cập nhật thành công.", true);
                
            });
        }
    }
}