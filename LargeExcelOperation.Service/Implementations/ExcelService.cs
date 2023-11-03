using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using LargeExcelOperation.Core.Models;
using LargeExcelOperation.Service.Helpers;
using LargeExcelOperation.Service.Interfaces;
using LargeXlsx;
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
            _logger.LogInformation("{MethodName} Excel operation start", methodName);
            var watch = Stopwatch.StartNew();
            IWorkbook workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet("Sheet 1");
            watch.Stop();
            _logger.LogInformation("{MethodName} initialize Excel app, transition time: {Time}", methodName,
                watch.ElapsedMilliseconds);
            watch.Restart();

            var firstRow = sheet.CreateRow(0);
            var style = workbook.CreateCellStyle();
            style.FillForegroundColor = IndexedColors.Black.Index;
            style.FillPattern = FillPattern.SolidForeground;
            var font = workbook.CreateFont();
            font.Color = IndexedColors.White.Index;
            style.SetFont(font);

            var properties = PropertyHelper.GetModelProperties<TEntity>();
            var titles = PropertyHelper.GetExcelTitleModels<TEntity>(properties);
            for (int i = 0; i < titles.Length; i++)
            {
                var cell = firstRow.CreateCell(i);
                cell.SetCellValue(titles.ElementAt(i).Title);
                cell.CellStyle = style;
            }

            watch.Stop();
            _logger.LogInformation("{MethodName} create first row with specific color, transition time: {Time}",
                methodName, watch.ElapsedMilliseconds);
            watch.Restart();

            int rowNumber = 1;
            foreach (var item in data)
            {
                var row = sheet.CreateRow(rowNumber);
                for (int i = 0; i < properties.Length; i++)
                {
                    var cell = row.CreateCell(i);
                    var property = properties[i];
                    var value = property.GetValue(item);
                    cell.SetCellValue(value?.ToString());
                }

                rowNumber++;
            }

            watch.Stop();
            _logger.LogInformation("{MethodName} create other rows, transition time: {Time}", methodName,
                watch.ElapsedMilliseconds);
            watch.Restart();

            await using var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);
            result = memoryStream.ToArray();
            watch.Stop();
            _logger.LogInformation("{MethodName} excel save on memory stream, transition time: {Time}", methodName,
                watch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MethodName} Exception", methodName);
        }

        return result;
    }

    public async Task<byte[]> CreateExcelWithLargeXlsxAsync<TEntity>(IEnumerable<TEntity> data)
    {
        string methodName = $"{nameof(ExcelService)} - {nameof(CreateExcelWithNpoiAsync)} - {Guid.NewGuid()}";
        var result = Array.Empty<byte>();
        try
        {
            _logger.LogInformation("{MethodName} Excel operation start", methodName);
            var watch = Stopwatch.StartNew();
            await using var stream = new MemoryStream();
            using var xlsxWriter = new XlsxWriter(stream);
            var sheet = xlsxWriter.BeginWorksheet("Sheet 1");

            watch.Stop();
            _logger.LogInformation("{MethodName} initialize Excel app, transition time: {Time}", methodName,
                watch.ElapsedMilliseconds);
            watch.Restart();

            var properties = PropertyHelper.GetModelProperties<TEntity>();
            var titles = PropertyHelper.GetExcelTitleModels<TEntity>(properties);
            var firstRow = sheet.BeginRow();
            var whiteFont = new XlsxFont("Calibri", 12, Color.White, bold: true);
            var blackFill = new XlsxFill(Color.Black);
            var headerStyle = new XlsxStyle(whiteFont, blackFill, XlsxBorder.None, XlsxNumberFormat.General,
                XlsxAlignment.Default);
            foreach (var t in titles)
            {
                firstRow.Write(t.Title, headerStyle);
            }
            
            watch.Stop();
            _logger.LogInformation("{MethodName} create first row with specific color, transition time: {Time}",
                methodName, watch.ElapsedMilliseconds);
            watch.Restart();

            foreach (var item in data)
            {
                var row = sheet.BeginRow();
                foreach (var property in properties)
                {
                    var value = property.GetValue(item);
                    row.Write(value?.ToString());
                }
            }
            
            watch.Stop();
            _logger.LogInformation("{MethodName} create other rows, transition time: {Time}", methodName,
                watch.ElapsedMilliseconds);
            watch.Restart();

            result = stream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{MethodName} Exception", methodName);
        }

        return result;
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