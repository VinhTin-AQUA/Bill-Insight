using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using BillInsight.Helpers;
using BillInsight.Models.AddInvoices;
using BillInsight.Services;
using ReactiveUI;
using Splat;
using System.Globalization;
using System.Linq;

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
        
        // public ObservableCollection<HHDVu> HHDVuList { get; set; } = [];
        private ObservableCollection<HHDVu> _hhdVuList = new();
        public ObservableCollection<HHDVu> HHDVuList
        {
            get => _hhdVuList;
            set => this.RaiseAndSetIfChanged(ref _hhdVuList, value);
        }
        
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
        public ReactiveCommand<Unit, Unit> AddInvoiceDetailCommand { get; set; }
        public ReactiveCommand<string, Unit> RemoveInvoiceDetailCommand { get; set; }

        #endregion
        
        #region 
        
        public CookieModel? cookieModel { get; set; }
        
        #endregion

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
                // Console.WriteLine(AddInvoiceModel.CaptchaCode);
                // Console.WriteLine(AddInvoiceModel.MaCT);
                // Console.WriteLine(AddInvoiceModel.MaHD);
                //
                // if (cookieModel == null)
                // {
                //     return;
                // }
                // var (_, _, urlTaiXML) = await BachHoaXanhService.SendAPI(cookieModel.SvID, 
                //     cookieModel.ASPNET_SessionId, 
                //     AddInvoiceModel.CaptchaCode, 
                //     AddInvoiceModel.MaCT, 
                //     AddInvoiceModel.MaHD
                // );
                //
                // var xmlPath = await BachHoaXanhService.DownloadXMLFile(urlTaiXML);
                string xmlPath = "/home/newtun/.local/share/BillInsight/TempData/temp.xml";
                
                (NBan nban, List<HHDVu> hhdvuList, TToan ttoan) = await BachHoaXanhService.GetXAMLData(xmlPath);
                NBan =  nban;
                HHDVuList = new ObservableCollection<HHDVu>(hhdvuList);
                TToan =  ttoan;
                Console.WriteLine(HHDVuList.Count);
            });

            AddInvoiceDetailCommand = ReactiveCommand.Create(() =>
            {
                HHDVuList.Add(new HHDVu());
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
                
            SaveInvoiceCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                FolderHelpers.CleanTempFolder();
            });
        }
        

    }
}