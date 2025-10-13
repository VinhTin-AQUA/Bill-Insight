using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Avalonia.Media.Imaging;
using BillInsight.Helpers;
using ReactiveUI;

namespace BillInsight.Models.AddInvoices
{
    public class AddInvoiceModel : ReactiveObject
    {
        private string _MaHD = "";
        public string MaHD
        {
            get => _MaHD;
            set => this.RaiseAndSetIfChanged(ref _MaHD, value);
        }

        private string _MaCT = "";
        public string MaCT
        {
            get => _MaCT;
            set =>  this.RaiseAndSetIfChanged(ref _MaCT, value);
        }
        
        private string _filePath = "";
        public string FilePath
        {
            get => _filePath;
            set
            {
                this.RaiseAndSetIfChanged(ref _filePath, value); 
            }
        }
        
        private string _captchaCode = "";
        public string CaptchaCode
        {
            get => _captchaCode;
            set => this.RaiseAndSetIfChanged(ref _captchaCode, value);
        }

        private string _captchaPath = "";
        public string CaptchaPath
        {
            get => _captchaPath;
            set => this.RaiseAndSetIfChanged(ref _captchaPath, value);
        }
    }
    
    public class NBan : ReactiveObject
    {
        private string _ten = "";
        public string Ten
        {
            get => _ten;
            set => this.RaiseAndSetIfChanged(ref _ten, value);
        }
        
        private string _mst = "";
        public string MST
        {
            get => $"MST: {_mst}";
            set => this.RaiseAndSetIfChanged(ref _mst, value);
        }
        
        private string _dchi = "";
        public string DChi
        {
            get => $"Địa chỉ: {_dchi}";
            set => this.RaiseAndSetIfChanged(ref _dchi, value);
        }

        private string _sDThoai = "";
        public string SDThoai
        {
            get => $"ĐT: {_sDThoai}";
            set => this.RaiseAndSetIfChanged(ref _sDThoai, value);
        }
        
        public ObservableCollection<TTKhac> TTKhac { get; set; } = [];
    }

    public class TTKhac : ReactiveObject
    {
        private string _TTruong = "";

        public string TTruong
        {
            get => _TTruong; 
            set => this.RaiseAndSetIfChanged(ref _TTruong, value);
        }
        
        private string _KDLieu = "";
        public string KDLieu { 
            get => _KDLieu;
            set => this.RaiseAndSetIfChanged(ref _KDLieu, value);
        }
        
        private string _DLieu = "";
        public string DLieu
        {
            get => _DLieu;
            set => this.RaiseAndSetIfChanged(ref _DLieu, value);
        }
    }

    public class HHDVu : ReactiveObject
    {
        public string Id { get; set; }
        
        private string _MHHDVu = "";
        public string MHHDVu
        {
            get => _MHHDVu;
            set => this.RaiseAndSetIfChanged(ref _MHHDVu, value);
        }

        private string _THHDVu = "";
        public string THHDVu
        {
            get => _THHDVu;
            set =>  this.RaiseAndSetIfChanged(ref _THHDVu, value);
        }

        private string _DVTinh = "";
        public string DVTinh
        {
            get => _DVTinh;
            set =>  this.RaiseAndSetIfChanged(ref _DVTinh, value);
        }

        private int _SLuong;
        public int SLuong
        {
            get => _SLuong;
            set => this.RaiseAndSetIfChanged(ref _SLuong, value);
        }
        
        private decimal _DGia;
        public decimal DGia
        {
            get => _DGia;
            set => this.RaiseAndSetIfChanged(ref _DGia, value);
        }
        
        private decimal _ThTien;
        public decimal ThTien
        {
            get => _ThTien;
            set => this.RaiseAndSetIfChanged(ref _ThTien, value);
        }
        
        private string _TSuat = "";
        public string TSuat
        {
            get => _TSuat;
            set => this.RaiseAndSetIfChanged(ref _TSuat, value);
        }
        
        private decimal _ThTienSauLaiSuat;
        public decimal ThTienSauLaiSuat
        {
            get => _ThTienSauLaiSuat;
            set => this.RaiseAndSetIfChanged(ref _ThTienSauLaiSuat, value);
        }

        public HHDVu(string mHHDVu, string tHHDVu, string dVTinh, int sLuong, decimal dGia, decimal thTien, string tSuat)
        {
            Id = Guid.NewGuid().ToString();
            MHHDVu = mHHDVu;
            THHDVu = tHHDVu;
            DVTinh =  dVTinh;
            SLuong =  sLuong;
            DGia = dGia;
            ThTien = thTien;
            TSuat = tSuat;
            ThTienSauLaiSuat = Math.Round(ThTien + ThTien * NumberHelpers.ParsePercentage(TSuat));
        }
        
        public HHDVu()
        {
            Id = Guid.NewGuid().ToString();
            THHDVu = "";
            ThTienSauLaiSuat = 0;
        }
    }

    public class LTSuat : ReactiveObject
    {
        private string _tSuat = string.Empty;
        public string TSuat
        {
            get => _tSuat;
            set => this.RaiseAndSetIfChanged(ref _tSuat, value);
        }

        private decimal _thTien;
        public decimal ThTien
        {
            get => _thTien;
            set => this.RaiseAndSetIfChanged(ref _thTien, value);
        }

        private decimal _tThue;
        public decimal TThue
        {
            get => _tThue;
            set => this.RaiseAndSetIfChanged(ref _tThue, value);
        }
    }

    public class TToan : ReactiveObject
    {
        private List<LTSuat> _tHTTLTSuat = new();
        public List<LTSuat> THTTLTSuat
        {
            get => _tHTTLTSuat;
            set => this.RaiseAndSetIfChanged(ref _tHTTLTSuat, value);
        }

        private decimal _tgTCThue;
        public decimal TgTCThue
        {
            get => _tgTCThue;
            set => this.RaiseAndSetIfChanged(ref _tgTCThue, value);
        }

        private decimal _tgTThue;
        public decimal TgTThue
        {
            get => _tgTThue;
            set => this.RaiseAndSetIfChanged(ref _tgTThue, value);
        }

        private decimal _tgTTTBSo;
        public decimal TgTTTBSo
        {
            get => _tgTTTBSo;
            set => this.RaiseAndSetIfChanged(ref _tgTTTBSo, value);
        }

        private string _tgTTTBChu = string.Empty;
        public string TgTTTBChu
        {
            get => _tgTTTBChu;
            set => this.RaiseAndSetIfChanged(ref _tgTTTBChu, value);
        }

        private List<TTKhac> _ttKhac = new();
        public List<TTKhac> TTKhac
        {
            get => _ttKhac;
            set => this.RaiseAndSetIfChanged(ref _ttKhac, value);
        }
    }

    public class CookieModel : ReactiveObject
    {
        public string SvID { get; set; } = string.Empty;
        public string ASPNET_SessionId { set; get; } = string.Empty;
        public string CaptchaPath { get; set; } = string.Empty;
    }
}