# BillInsight

## Test data

-   CT: OV203555509416602
-   InvoiceCode: BF394B7E06

## Build Steps

-   Call API lấy prefix
-   Call API lấy captcha
-   nhập mã CT, captchae và mã truy xuất hóa đơn
-   call api
-   lấy dữ liệu

    -   tải xml và truy xuất dữ liệu
    -   đọc từ file pdf

-   gọi api chỉnh sửa sheet
-   lưu ảnh vào cloudinary

## Setup

### Setup dự án google cloud console

#### Create new project

-   Tạo Project
-   Bật API

    -   Vào APIs & Services → Library.
    -   Bật Google Sheets API (và Google Drive API nếu cần).

#### AUth

-   Cách 1: UserCredential (OAuth 2.0 cho user login)

    -   Vào APIs & Services → Credentials → Create Credentials → OAuth Client ID.
    -   Chọn Application type = Desktop app.
    -   Tải file **credentials.json** về.

-   Cách 2:(B) GoogleCredential (Service Account – không cần user login)

    -   Vào APIs & Services → Credentials → Create Credentials → Service Account.
    -   Tạo service account, đặt tên.
    -   Download file **service_account.json**.
    -   Lấy email của service account (dạng **xxx@project-id.iam.gserviceaccount.com**).
    -   Chia sẻ Google Sheet cho email này với quyền Editor.

### Code C# để sử dụng Google Sheets API

-   Tải thư viện

    ```bash
    Install-Package Google.Apis.Sheets.v4
    Install-Package Google.Apis.Drive.v3
    Install-Package Google.Apis.Auth
    ```

#### GoogleCredential

-   Sử dụng với GoogleCredential

    ```csharp

    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Google.Apis.Util.Store;
    using System.IO;
    using System.Threading;

    GoogleCredential credential;
    using (var stream = new FileStream("/home/newtun/Desktop/TestAPI/TestAPI/secrets/billinsight-0b2c14cec552.json", FileMode.Open, FileAccess.Read))
    {
        credential = GoogleCredential.FromStream(stream)
        .CreateScoped(SheetsService.Scope.Spreadsheets);
    }

    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = "BillInsight",
    });

    // - Đọc dữ liệu
    const string spreadsheetId = "1D4UeZBozLOjiIlhJ-YSuok-MqIJDCYicoI807K0tj1o";
    String range = "Sheet1!A1";

    var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
    var response = request.Execute();

    var values = response.Values;
    if (values != null && values.Count > 0)
    {
        foreach (var row in values)
        Console.WriteLine(string.Join(" | ", row));
    }
    ```

    -   Không có popup login.
    -   Chỉ chạy được nếu Google Sheet được share cho service account email.
    -   Thích hợp khi app chỉ dùng cho 1 tài khoản hệ thống cố định (ví dụ server app, automation).

#### UserCredential

-   Sử dụng với UserCredential

    ```csharp
    using Google.Apis.Auth.OAuth2;
    using Google.Apis.Services;
    using Google.Apis.Sheets.v4;
    using Google.Apis.Sheets.v4.Data;
    using Google.Apis.Util.Store;
    using System.IO;
    using System.Threading;

    class Program
    {
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Google Sheets API C# Demo";

        static void Main(string[] args)
        {
            UserCredential credential;
            using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            string spreadsheetId = "YOUR_SHEET_ID";
            string range = "Sheet1!A1:D5";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();

            foreach (var row in response.Values)
                System.Console.WriteLine(string.Join(" | ", row));
        }

    }
    ```

-   Lần đầu chạy sẽ mở trình duyệt → user đăng nhập Google → cấp quyền.
-   Token lưu ở token.json, lần sau không cần login lại.
-   Thích hợp khi app cần cho nhiều người dùng khác nhau chạy với tài khoản riêng của họ
-   Nếu trong môi trường phát triển, mặc định ứng dụng Unverified app, thì email đăng nhạp cần nằm trong danh sách Test users của ứng dụng mới có thể sử dụng app

-   Cách khắc phục nhanh (cho phát triển / testing)
    Vào Google Cloud Console → APIs & Services → OAuth consent screen.
    Ở bước User type, bạn chọn External (nếu muốn cho account ngoài tổ chức G-Suite).
    Trong phần Test users, thêm email abc@gmail.com của bạn (và các email tester khác nếu cần).
    👉 Sau đó bạn có thể đăng nhập mà không bị lỗi.

-   Nếu muốn khắc phục lâu dài (cho triển khai chính thức) cần verify app

### So sánh UserCredential vs GoogleCredential

-   Tiêu chí UserCredential (OAuth2) GoogleCredential (Service Account)
-   Đăng nhập Người dùng phải login Google (một lần, token được lưu lại) Không cần login, chạy tự động
-   Đối tượng sử dụng Ứng dụng client dành cho nhiều người dùng Ứng dụng server/automation, 1 tài khoản cố định
-   Quyền truy cập Dựa trên tài khoản Google user đăng nhập Dựa trên email của service account (phải share tài liệu)
-   Triển khai Dễ dùng cho end-user (mỗi người dùng login bằng account của họ) Dễ dùng cho backend/system app (không ai phải login)
-   File credentials credentials.json (OAuth client ID) **service_account.json** (private key + email service)

### features

-   Settings
-   Themes
-   Nhập keys

    -   nhập file **service_account.json**
    -   nhập ID của spreadsheet
    -   Key cloudinary
    -   Key supabase

-   Lưu ảnh
-   Xem tổng số tiền, số tiền đã dùng, số tiền còn lại
-   xem danh sách ảnh hóa đơn, theo ngày
-   Xem chi tiết hóa đơn
-   Chuyển đổi giữa các sheet
-   Template sheet

#### Các bước thêm hóa đơn

-   Tải file ảnh
-   Get GetPrefixAndSvID
-   GetCaptchaAndASPSession
-   nhập mã đơn, mã hóa đơn, captcha
-   SendAPI
-   Xem trước dữ liệu, được quyền chỉnh sửa, tổng tiền
-   Chọn ngày hóa đơn
-   Cập nhật spreadsheet và lưu ảnh hóa đơn tại cloudinary
-   có thể nhập dữ liệu bằng tay
-   Loại tiền mặt hoặc ngân hàng
-   triển khai cho nhiều loại hóa đơn khác

#### Backend

-   Lưu data dưới dạng json
-   Ứng dụng được cài đặt trong app data
-   Android, Windows, Linux
-   Ảnh lưu tại clouidinary
-   Key lưu tại local
-   Attachment lưu tại supabase

#### màng hình

-   Thống kê: theo ngày, theo tuần, theo tháng, tổng chi tiêu
-   Xem chi tiết hóa đơn, xem từng sheet
-   Xem danh sách ảnh hóa đơn
-   Nhập hóa đơn
-   Thông tin - nhập ID của spreadsheet, danh sách sheet, thao tác với sheet
-   Settings
    -   Set theme
    -   Clear all data
    -   Keys
        -   nhập file service_account.json
        -   Key cloudinary
        -   Key supabase

