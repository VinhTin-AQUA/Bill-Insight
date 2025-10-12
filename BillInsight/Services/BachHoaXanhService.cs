using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using BillInsight.Helpers;
using BillInsight.Models.AddInvoices;

namespace BillInsight.Services
{
    public class BachHoaXanhService
    {
        private static readonly string ApiUrl = "https://hddt.bachhoaxanh.com";
        
        public async Task<CookieModel?> GetCaptchaAndASPSession()
        {
            // string captcheUrl = "https://hddt.bachhoaxanh.com";
            (string? svID, string? prefix) = await GetPrefixAndSvID();
            if (svID == null || prefix == null)
            {
                return null;
            }
            
            var baseAddress = new Uri(ApiUrl); 
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseAddress, new Cookie("SvID", svID));
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                UseDefaultCredentials = false
            };

            using var client = new HttpClient(handler);
            client.BaseAddress = baseAddress;
            string endpoint = $"/home/getcaptchaimage?prefix={prefix}";
            byte[] response = await client.GetByteArrayAsync(endpoint); // gọi GET

            String captchaPath =  Path.Combine(FolderHelpers.GetFolder(AppFolders.TempData), $"{Guid.NewGuid().ToString()}.jpeg");
            await File.WriteAllBytesAsync(captchaPath, response);
            
            CookieCollection cookies = cookieContainer.GetCookies(baseAddress);
            string? aspSessionId = cookies["ASP.NET_SessionId"]?.Value;
            return new()
            {
                SvID = svID,
                ASPNET_SessionId = aspSessionId ?? "",
                CaptchaPath = captchaPath
            };
        }
        
        public async Task<(string, string, string)> SendAPI(string svId, string aspSessionId, string captcha, string phone, string invoiceNum)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{ApiUrl}/Home/ListInvoice");
            request.Headers.Add("accept", "*/*");
            request.Headers.Add("origin", "https://hddt.bachhoaxanh.com");
            request.Headers.Add("sec-ch-ua", "\"Google Chrome\";v=\"141\", \"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"141\"");
            request.Headers.Add("sec-ch-ua-platform", "\"Linux\"");
            request.Headers.Add("sec-fetch-mode", "cors");
            request.Headers.Add("sec-fetch-site", "same-origin");
            request.Headers.Add("user-agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/141.0.0.0 Safari/537.36");
            request.Headers.Add("x-requested-with", "XMLHttpRequest");
            request.Headers.Add("cookie", $"SvID={svId}; ASP.NET_SessionId={aspSessionId}");

            request.Content = new StringContent($"phone={phone}&invoiceNum={invoiceNum}&captcha={captcha}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded; charset=UTF-8");

            HttpResponseMessage response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            // Console.WriteLine(responseBody);

            var (urlXem, urlTaiHDChuyenDoi, urlTaiXML) = GetURLs(responseBody);
            return (urlXem, urlTaiHDChuyenDoi, urlTaiXML);
        }
        
        public async Task<string> DownloadXMLFile(string xmlUrl)
        {
            string filePath = Path.Combine(FolderHelpers.GetFolder(AppFolders.TempData), $"{Guid.NewGuid().ToString()}.xml");
            using (HttpClient client = new HttpClient())
            {
                // Gọi API
                Console.WriteLine("Đang gọi API...");
                byte[] imageBytes = await client.GetByteArrayAsync(xmlUrl);

                // Lưu ảnh vào file
                Console.WriteLine("Đang lưu ảnh...");
                await File.WriteAllBytesAsync(filePath, imageBytes);
                Console.WriteLine($"Ảnh đã được lưu thành công tại: {filePath}");
            }

            return filePath;
        }
        
        public async Task<(NBan, List<HHDVu>, TToan)> GetXAMLData(string xmlPath)
        {
            using var stream = File.OpenRead(xmlPath);

            var doc = await XDocument.LoadAsync(
                stream,
                LoadOptions.None,
                default // CancellationToken
            );

            // Lấy thông tin NBan
            var nbanElement = doc.Descendants("NBan").FirstOrDefault();
            
            var nban = new NBan
            {
                Ten = nbanElement!.Element("Ten")!.Value,
                MST = nbanElement.Element("MST")!.Value,
                DChi = nbanElement.Element("DChi")!.Value,
                SDThoai = nbanElement.Element("SDThoai")!.Value,
                TTKhac = new ObservableCollection<TTKhac>(
                    nbanElement.Element("TTKhac")!
                        .Elements("TTin")
                        .Select(x => new TTKhac
                        {
                            TTruong = x.Element("TTruong")!.Value,
                            KDLieu = x.Element("KDLieu")!.Value,
                            DLieu = x.Element("DLieu")!.Value
                        })
                )
            };

            // Lấy danh sách HHDVu
            var hhdvuList = doc.Descendants("HHDVu").Select(x => 
                new HHDVu(
                    x.Element("MHHDVu")!.Value, 
                    x.Element("THHDVu")!.Value, 
                    x.Element("DVTinh")!.Value, 
                    int.TryParse(x.Element("SLuong")!.Value, out var sl) ? sl : 0, 
                    decimal.TryParse(x.Element("DGia")!.Value, out var dg) ? dg : 0,
                    decimal.TryParse(x.Element("ThTien")!.Value, out var tt) ? tt : 0, 
                    x.Element("TSuat")!.Value)
                )
                .ToList();

            // Lấy thông tin TToan
            var ttoanElement = doc.Descendants("TToan").FirstOrDefault();
            var ttoan = new TToan
            {
                THTTLTSuat = ttoanElement!.Element("THTTLTSuat")!
                    .Elements("LTSuat")
                    .Select(x => new LTSuat
                    {
                        TSuat = x.Element("TSuat")!.Value,
                        ThTien = decimal.TryParse(x.Element("ThTien")?.Value, out var t) ? t : 0,
                        TThue = decimal.TryParse(x.Element("TThue")?.Value, out var th) ? th : 0
                    }).ToList(),
                TgTCThue = decimal.TryParse(ttoanElement.Element("TgTCThue")?.Value, out var tc) ? tc : 0,
                TgTThue = decimal.TryParse(ttoanElement.Element("TgTThue")?.Value, out var tth) ? tth : 0,
                TgTTTBSo = decimal.TryParse(ttoanElement.Element("TgTTTBSo")?.Value, out var ttb) ? ttb : 0,
                TgTTTBChu = ttoanElement.Element("TgTTTBChu")!.Value,
                TTKhac = ttoanElement.Element("TTKhac")!
                    .Elements("TTin")
                    .Select(x => new TTKhac
                    {
                        TTruong = x.Element("TTruong")!.Value,
                        KDLieu = x.Element("KDLieu")!.Value,
                        DLieu = x.Element("DLieu")!.Value
                    }).ToList()
            };

            // Demo in ra
            // Console.WriteLine("---- NBan ----");
            // Console.WriteLine($"{nban.Ten} | MST: {nban.MST} | Địa chỉ: {nban.DChi} | ĐT: {nban.SDThoai}");
            // foreach (var t in nban.TTKhac)
            //     Console.WriteLine($"TTKhac: {t.TTruong} = {t.DLieu}");
            //
            // Console.WriteLine("\n---- HHDVu ----");
            // foreach (var h in hhdvuList)
            //     Console.WriteLine(
            //         $"{h.MHHDVu} | {h.THHDVu} | SL: {h.SLuong} | Giá: {h.DGia} | Thành tiền: {h.ThTien} | Thuế: {h.TSuat}");
            //
            // Console.WriteLine("\n---- TToan ----");
            // foreach (var lt in ttoan.THTTLTSuat)
            //     Console.WriteLine($"Thuế suất: {lt.TSuat}, Tiền: {lt.ThTien}, Thuế: {lt.TThue}");
            // Console.WriteLine($"Tổng cộng chưa thuế: {ttoan.TgTCThue}");
            // Console.WriteLine($"Tiền thuế: {ttoan.TgTThue}");
            // Console.WriteLine($"Tổng thanh toán (số): {ttoan.TgTTTBSo}");
            // Console.WriteLine($"Tổng thanh toán (chữ): {ttoan.TgTTTBChu}");

            return (nban, hhdvuList, ttoan);
        }

        #region private methods

        private static (string, string, string) GetURLs(string html)
        {
            string pattern = @"<a[^>]*href=""([^""]+)""[^>]*>\s*(XEM|Tải HĐ chuyển đổi|TẢI XML)\s*</a>";
            MatchCollection matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);
            
            string urlXem = matches[0].Groups[1].Value;
            string urlTaiHDChuyenDoi = matches[1].Groups[1].Value;
            string urlTaiXML = matches[2].Groups[1].Value;
            
            return (urlXem, urlTaiHDChuyenDoi, urlTaiXML);
        }
        
        public static async Task<(string?, string?)> GetPrefixAndSvID()
        {
            // string url = "https://hddt.bachhoaxanh.com/";
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                UseCookies = true,
                AllowAutoRedirect = true
            };
            
            using HttpClient client = new HttpClient(handler);
            HttpResponseMessage responseBHX = await client.GetAsync(ApiUrl);
            responseBHX.EnsureSuccessStatusCode();
            
            // Lấy cookie từ response
            Uri uri = new Uri(ApiUrl);
            CookieCollection cookies = cookieContainer.GetCookies(uri);
            string? svID = cookies["SvID"]?.Value;
            
            string htmlContent = await responseBHX.Content.ReadAsStringAsync();
            string pattern = @"<img\s+[^>]*src=""/home/getcaptchaimage\?prefix=(\d+)""";
            Match match = Regex.Match(htmlContent, pattern);
            string prefix = ""; // for captcha
            if (match.Success)
            {
                prefix = match.Groups[1].Value;
            }
            return (svID, prefix);
        }

        #endregion
    }
}