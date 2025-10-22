using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BillInsight.Models.SpreadSheetInfos;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace BillInsight.Services
{
    public class GoogleSpreadsheetService
    {
        private SheetsService service = null!;
        // private const string CredentialFilePath = "";
        private const string ApplicationName = "BillInsight";
        
        // id của toàn bộ spreadsheet (id của file google trang tính)
        private string SpreadsheetId = "";
        
        // public GoogleSpreadsheetService()
        // {
        //     
        // }

        public bool Init(string credentialFilePath, string spreadsheetId)
        {
            if (string.IsNullOrWhiteSpace(credentialFilePath) || string.IsNullOrEmpty(credentialFilePath))
            {
                return false;
            }
       
            try
            {
                GoogleCredential credential;
                using (var stream =
                       new FileStream(credentialFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = CredentialFactory.FromStream<ServiceAccountCredential>(stream).ToGoogleCredential();
                }

                service = new SheetsService(new BaseClientService.Initializer
                {
                    HttpClientInitializer = credential,
                    ApplicationName = ApplicationName
                });
                SpreadsheetId = spreadsheetId;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T?> ReadDataFromCell<T>(string sheetName, string rangeReference)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference);
            // Đọc dữ liệu tại cell cụ thể
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            var response = await request.ExecuteAsync();
            var values = response.Values;
            if (values != null && values.Count > 0 && values[0].Count > 0)
            {
                var cellValue = values[0][0];
                try
                {
                    if (cellValue == null)
                        return default;
                    var converted = (T)Convert.ChangeType(cellValue, typeof(T));
                    return converted;
                }
                catch
                {
                    return default;
                }
            }
            return default;
        }

        public async Task<bool> WriteDataToCell(string sheetName, string rangeReference, object data)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference); // string range = "Sheet1!C5"; 
            var valueRange = new ValueRange();
            var values = new List<IList<object>>
            {
                new List<object> { data }  // Chỉ 1 giá trị
            };
            
            valueRange.Values = values;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }

        public async Task<List<T>> GetDataRow<T>(string sheetName, string rangeReference)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference); // string range = "Sheet1!A2:C5";
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            IList<IList<object>>? values = response.Values;

            if (values == null || values.Count == 0)
            {
                return [];
            }

            var firstRow = values[0];
            try
            {
                var result = firstRow
                    .Select(value => (T)Convert.ChangeType(value, typeof(T)))
                    .ToList();
                return result;
            }
            catch
            {
                return [];
            }
        }
        
        public async Task<List<T>> GetDataColumn<T>(string sheetName, string rangeReference)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference);
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            IList<IList<object>>? values = response.Values;

            if (values == null || values.Count == 0)
            {
                return [];
            }

            try
            {
                List<T> result = new List<T>();
                foreach (var row in values)
                {
                    if (row.Count > 0)
                    {
                        var value = row[0];
                        result.Add((T)Convert.ChangeType(value, typeof(T)));
                    }
                }

                return result;
            }
            catch
            {
                return [];
            }
        }
        
        public async Task<List<List<T>>> GetDataMatrix<T>(string sheetName, string rangeReference, bool skipHeader = false)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference);
            var request = service.Spreadsheets.Values.Get(SpreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            IList<IList<object>>? values = response.Values;
            var result = new List<List<T>>();
            if (values == null || values.Count == 0)
            {
                return result; // trả về danh sách rỗng
            }
            
            // Nếu bỏ header thì bỏ qua hàng đầu tiên
            if (skipHeader)
                values = values.Skip(1).ToList();
            
            foreach (var row in values)
            {
                var rowList = new List<T>();
                foreach (var cell in row)
                {
                    try
                    {
                        rowList.Add((T)Convert.ChangeType(cell, typeof(T)));
                    }
                    catch
                    {
                        rowList.Add(default!);
                    }
                }
                result.Add(rowList);
            }
            return result;
        }

        public async Task<bool> WriteDataToMatrix(string sheetName, string rangeReference, List<IList<object>> data)
        {
            CheckInitialService();
            // var values = new List<IList<object>>
            // {
            //     new List<object> { "ID", "Name", "Age" },
            //     new List<object> { 1, "Alice", 25 },
            //     new List<object> { 2, "Bob", 30 },
            //     new List<object> { 3, "Carol", 28 }
            // };
            
            string range = GetRange(sheetName, rangeReference); // var range = "Sheet1!A1:C4"; // ghi từ A1 tới C4 trong sheet "Sheet1"
            var valueRange = new ValueRange();
            valueRange.Values = data;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }
        
        public async Task<bool> WriteDataToRow(string sheetName, string rangeReference, List<object> data)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference); // var range = "Sheet1!A1:A10"; // ghi từ A1 tới A10 trong sheet "Sheet1"
            var valueRange = new ValueRange();
            var values = new List<IList<object>>
            {
                data
            };
            valueRange.Values = values;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }
        
        public async Task<bool> WriteDataToColumn(string sheetName, string rangeReference, List<object> data)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference); // Tạo danh sách giá trị theo chiều dọc (mỗi phần tử là 1 dòng, 1 ô)
            var values = new List<IList<object>>();
            foreach (var item in data)
            {
                values.Add(new List<object> { item });
            }
            var valueRange = new ValueRange
            {
                Values = values
            };

            // Gửi yêu cầu cập nhật dữ liệu vào Google Sheets
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, SpreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }

        public async Task<bool> Append(string sheetName, string rangeReference, List<IList<object>> values)
        {
            CheckInitialService();
            
            string range = GetRange(sheetName, rangeReference); // "Sheet1!D1"
            var valueRange = new ValueRange();
            // var values = new List<IList<object>>
            // {
            //     new List<object> { "ID", "Name", "Age" },
            //     new List<object> { 1, "Alice", 25 },
            //     new List<object> { 2, "Bob", 30 },
            //     new List<object> { 3, "Carol", 28 }
            // };
            valueRange.Values = values;

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SpreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = await appendRequest.ExecuteAsync();
            return appendResponse.Updates.UpdatedCells != null && appendResponse.Updates.UpdatedCells > 0;
        }
        
        public async Task<List<SheetModel>> GetSheets()
        {
            CheckInitialService();
            
            var request = service.Spreadsheets.Get(SpreadsheetId);
            var response = await request.ExecuteAsync();
            List<SheetModel> sheets = [];
            foreach (var sheet in response.Sheets)
            {
                var properties = sheet.Properties;
                sheets.Add(new() {Title = properties.Title, Id = properties.SheetId});
            }
            return sheets;
        }

        public async Task<SheetModel?> CreateSheet(string sheetName)
        {
            CheckInitialService();
            
            var addSheetRequest = new AddSheetRequest
            {
                Properties = new SheetProperties
                {
                    Title = sheetName
                }
            };
            
            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new[]
                {
                    new Request { AddSheet = addSheetRequest }
                }
            };
            
            var request = service.Spreadsheets.BatchUpdate(batchUpdateRequest, SpreadsheetId);
            var response = await request.ExecuteAsync();

            var SheetId = response.Replies[0].AddSheet.Properties.SheetId;
            var Title = response.Replies[0].AddSheet.Properties.Title;
            
            bool added = response.Replies != null && response.Replies.Count > 0 && response.Replies[0].AddSheet != null;
            if (!added)
            {
                return null;
            }

            return new()
            {
                Title = Title,
                Id = SheetId
            };
        }

        public async Task<bool> RemoveSheet(int sheetId)
        {
            CheckInitialService();
            
            var deleteSheetRequest = new Request()
            {
                DeleteSheet = new DeleteSheetRequest
                {
                    SheetId = sheetId
                }
            };

            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new List<Request> { deleteSheetRequest }
            };

            var request = service.Spreadsheets.BatchUpdate(batchUpdateRequest, SpreadsheetId);
            var response = await request.ExecuteAsync();
            bool removed = response.Replies != null && response.Replies.Count > 0 && response.Replies[0].AddSheet != null;
            return removed;
        }

        public async Task<SheetModel?> UpdateSheet(SheetModel sheet)
        {
            CheckInitialService();
            
            // Tạo request cập nhật tên sheet (KHÔNG phải thêm mới)
            var updateSheetRequest = new UpdateSheetPropertiesRequest
            {
                Properties = new SheetProperties
                {
                    SheetId = sheet.Id,   // ID của sheet cần đổi tên
                    Title = sheet.Title   // Tên mới
                },
                Fields = "title"  // Chỉ cập nhật trường 'title'
            };

            var batchUpdateRequest = new BatchUpdateSpreadsheetRequest
            {
                Requests = new[]
                {
                    new Request { UpdateSheetProperties = updateSheetRequest }
                }
            };

            var request = service.Spreadsheets.BatchUpdate(batchUpdateRequest, SpreadsheetId);
            var response = await request.ExecuteAsync();
            bool updated = response.Replies != null && response.Replies.Count > 0;
            if (!updated)
            {
                return null;
            }

            return new SheetModel
            {
                Id = sheet.Id,
                Title = sheet.Title
            };
        }
        
        #region private methods

        private void CheckInitialService()
        {
            if (service == null)
            {
                throw new Exception("Google Spreadsheet Service is not initialized. Please config credential path.");
            }
        }
        
        private string GetRange(string sheetName, string rangeReference)
        {
            return $"{sheetName}!{rangeReference}";
        }
        
        #endregion
    }
}
