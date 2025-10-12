using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Helpers;
using BillInsight.Models.AddInvoices;
using BillInsight.Services;
using ReactiveUI;
using Splat;

namespace BillInsight.ViewModels
{
    public class AddInvoiceViewModel : ViewModelBase
    {
        #region props
        
        private NBan _NBan;
        public NBan NBan
        {
            get => _NBan;
            set => this.RaiseAndSetIfChanged(ref _NBan, value);
        }
        
        public ObservableCollection<HHDVu> HHDVuList { get; set; } = [];

        private TToan _TToan;
        public TToan TToan
        {
            get => _TToan;
            set => this.RaiseAndSetIfChanged(ref _TToan, value);
        }
        
        private AddInvoiceModel _AddInvoiceModel;
        public AddInvoiceModel AddInvoiceModel
        {
            get => _AddInvoiceModel;
            set => this.RaiseAndSetIfChanged(ref _AddInvoiceModel, value);
        }
        
        #endregion

        #region commands

        public ReactiveCommand<Unit, Unit> RemoveImageCommand { get; set; }
        public ReactiveCommand<Unit, Unit> GetCaptchaCommand { get; set; }
        public ReactiveCommand<Unit, Unit> SendAPICommand { get; set; }
        public ReactiveCommand<Unit, Unit> SaveInvoiceCommand { get; set; }

        #endregion
        
        #region 
        
        public CookieModel? cookieModel { get; set; }
        
        #endregion
        
        // public ObservableCollection<ProductItem> Products { get; set; } = [];

        #region servives

        public BachHoaXanhService BachHoaXanhService { get; set; }

        #endregion
        
        public AddInvoiceViewModel()
        {
            BachHoaXanhService = Locator.Current.GetService<BachHoaXanhService>()!;
            InitProps();
            InitCommands();
        }

        private void InitProps()
        {
            NBan = new NBan();
            TToan = new TToan();
            AddInvoiceModel = new AddInvoiceModel();
        }
        
        private void InitCommands()
        {
            GetCaptchaCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                cookieModel = await BachHoaXanhService.GetCaptchaAndASPSession();
                if (cookieModel != null)
                {
                    AddInvoiceModel.CaptchaPath = cookieModel.CaptchaPath;
                }
            });

            SendAPICommand = ReactiveCommand.CreateFromTask(async () =>
            {
                if (cookieModel == null)
                {
                    return;
                }
                var (_, _, urlTaiXML) = await BachHoaXanhService.SendAPI(cookieModel.SvID, 
                    cookieModel.ASPNET_SessionId, 
                    AddInvoiceModel.CaptchaCode, 
                    AddInvoiceModel.MaCT, 
                    AddInvoiceModel.MaHD
                );
                
                var xmlPath = await BachHoaXanhService.DownloadXMLFile(urlTaiXML);
                (NBan nban, List<HHDVu> hhdvuList, TToan ttoan) = await BachHoaXanhService.GetXAMLData(xmlPath);
                NBan =  nban;
                HHDVuList = new ObservableCollection<HHDVu>(hhdvuList);
                TToan =  ttoan;
                Console.WriteLine(HHDVuList.Count);
            });

            SaveInvoiceCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                FolderHelpers.CleanTempFolder();
            });
        }
    }
}