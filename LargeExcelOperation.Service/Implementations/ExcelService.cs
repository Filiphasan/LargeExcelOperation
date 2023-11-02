using System.Diagnostics;
using System.Reflection;
using LargeExcelOperation.Service.Helpers;
using LargeExcelOperation.Service.Interfaces;
using Microsoft.Extensions.Logging;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace LargeExcelOperation.Service.Implementations;

public class ExcelService : IExcelService
{
    private readonly ILogger<ExcelService> _logger;

    public ExcelService(ILogger<ExcelService> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> CreateExcelWithNpoiAsync<TEntity>(IEnumerable<TEntity> data)
    {
        string methodName = $"{nameof(ExcelService)} - {nameof(CreateExcelWithNpoiAsync)} - {Guid.NewGuid()}";
        var result = Array.Empty<byte>();
        try
        {
            _logger.LogInformation("{methodName} Excel operation start.", methodName);
            var watch = Stopwatch.StartNew();
            watch.Start();
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet 1");
            watch.Stop();
            _logger.LogInformation("{methodName} initialize Excel app, transition time: {time}", methodName, watch.ElapsedMilliseconds);
            watch.Restart();

            var firstRow = sheet.CreateRow(0);
            var style = workbook.CreateCellStyle();
            style.FillForegroundColor = IndexedColors.Black.Index;
            style.FillPattern = FillPattern.SolidForeground;
            var font = workbook.CreateFont();
            font.Color = IndexedColors.White.Index;
            style.SetFont(font);

            var properties = PropertyHelper.GetModelProperties<TEntity>();
            var propertyInfos = properties as PropertyInfo[] ?? properties.ToArray();
            var titles = PropertyHelper.GetExcelTitleModels<TEntity>(propertyInfos);
            for (int i = 0; i < titles.Length; i++)
            {
                var cell = firstRow.CreateCell(i);
                cell.SetCellValue(titles.ElementAt(i).Title);
                cell.CellStyle = style;
            }
            
            watch.Stop();
            _logger.LogInformation("{methodName} create first row with specific color, transition time: {time}", methodName, watch.ElapsedMilliseconds);
            watch.Restart();

            int rowNumber = 1;
            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowNumber);
                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    var cell = row.CreateCell(i);
                    var property = propertyInfos.ElementAt(i);
                    var value = property.GetValue(item);
                    cell.SetCellValue(value?.ToString());
                }

                rowNumber++;
            }
            
            watch.Stop();
            _logger.LogInformation("{methodName} create other rows, transition time: {time}", methodName, watch.ElapsedMilliseconds);
            watch.Restart();

            await using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            result = memoryStream.ToArray();
            watch.Stop();
            _logger.LogInformation("{methodName} excel save on memory stream, transition time: {time}", methodName, watch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} Exception", methodName);
        }
        return result;
    }

    public async Task<byte[]> CreateExcelWithLargeXlsxAsync<TEntity>(IEnumerable<TEntity> data)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreateExcelWithEpPlusAsync<TEntity>(IEnumerable<TEntity> data)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreateExcelWithNpoiAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreateExcelWithLargeXlsxAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data)
    {
        throw new NotImplementedException();
    }

    public Task<byte[]> CreateExcelEpPlusAndMultiSheetAsync<TEntity>(IEnumerable<TEntity> data)
    {
        throw new NotImplementedException();
    }
}