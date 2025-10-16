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
        private SheetsService service;
        private const string CredentialFilePath = "/home/newtun/Desktop/Secrets/billinsight-0b2c14cec552.json";
        private const string ApplicationName = "BillInsight";
        
        // id của toàn bộ spreadsheet (id của file google trang tính)
        private const string spreadsheetId = "1D4UeZBozLOjiIlhJ-YSuok-MqIJDCYicoI807K0tj1o";
        
        public GoogleSpreadsheetService()
        {
            GoogleCredential credential;
            using (var stream =
                   new FileStream(CredentialFilePath,FileMode.Open, FileAccess.Read))
            {
                credential = CredentialFactory.FromStream<ServiceAccountCredential>(stream).ToGoogleCredential();
            }
            
            service = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });
        }

        public async Task<T?> ReadDataFromCell<T>(string sheetName, string rangeReference)
        {
            string range = GetRange(sheetName, rangeReference);
            // Đọc dữ liệu tại cell cụ thể
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
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
            string range = GetRange(sheetName, rangeReference); // string range = "Sheet1!C5"; 
            var valueRange = new ValueRange();
            var values = new List<IList<object>>
            {
                new List<object> { data }  // Chỉ 1 giá trị
            };
            
            valueRange.Values = values;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }

        public async Task<List<T>> GetDataRow<T>(string sheetName, string rangeReference)
        {
            string range = GetRange(sheetName, rangeReference); // string range = "Sheet1!A2:C5";
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
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
            string range = GetRange(sheetName, rangeReference);
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
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
        
        public async Task<List<List<T>>> GetDataMatrix<T>(string sheetName, string rangeReference)
        {
            string range = GetRange(sheetName, rangeReference);
            var request = service.Spreadsheets.Values.Get(spreadsheetId, range);
            ValueRange response = await request.ExecuteAsync();
            IList<IList<object>>? values = response.Values;
            var result = new List<List<T>>();
            if (values == null || values.Count == 0)
            {
                return result; // trả về danh sách rỗng
            }
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
            string range = GetRange(sheetName, rangeReference); // var range = "Sheet1!A1:C4"; // ghi từ A1 tới C4 trong sheet "Sheet1"
            var valueRange = new ValueRange();
            valueRange.Values = data;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }
        
        public async Task<bool> WriteDataToRow(string sheetName, string rangeReference, List<object> data)
        {
            string range = GetRange(sheetName, rangeReference); // var range = "Sheet1!A1:A10"; // ghi từ A1 tới A10 trong sheet "Sheet1"
            var valueRange = new ValueRange();
            var values = new List<IList<object>>
            {
                data
            };
            valueRange.Values = values;
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }
        
        public async Task<bool> WriteDataToColumn(string sheetName, string rangeReference, List<object> data)
        {
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
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var updateResponse = await updateRequest.ExecuteAsync();
            return updateResponse.UpdatedCells != null && updateResponse.UpdatedCells > 0;
        }

        public async Task<bool> Append(string sheetName, string rangeReference, List<IList<object>> values)
        {
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

            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendResponse = await appendRequest.ExecuteAsync();
            return appendResponse.Updates.UpdatedCells != null && appendResponse.Updates.UpdatedCells > 0;
        }
        
        public async Task<List<SheetModel>> GetSheets()
        {
            var request = service.Spreadsheets.Get(spreadsheetId);
            var response = await request.ExecuteAsync();
            List<SheetModel> sheets = [];
            foreach (var sheet in response.Sheets)
            {
                var properties = sheet.Properties;
                sheets.Add(new() {Title = properties.Title, Id = properties.SheetId});
            }
            return sheets;
        }

        public async Task<bool> CreateSheet(string sheetName)
        {
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
            
            var request = service.Spreadsheets.BatchUpdate(batchUpdateRequest, spreadsheetId);
            var response = await request.ExecuteAsync();
            bool added = response.Replies != null && response.Replies.Count > 0 && response.Replies[0].AddSheet != null;
            return added;
        }

        private string GetRange(string sheetName, string rangeReference)
        {
            return $"{sheetName}!{rangeReference}";
        }
    }
}
