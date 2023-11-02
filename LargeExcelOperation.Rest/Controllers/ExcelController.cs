using LargeExcelOperation.Core.Models;
using LargeExcelOperation.Core.Models.Report;
using LargeExcelOperation.Data.Contexts;
using LargeExcelOperation.Data.Helpers;
using LargeExcelOperation.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LargeExcelOperation.Rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcelController : ControllerBase
{
    private readonly LargeExcelDbContext _context;
    private readonly IReportService _reportService;

    public ExcelController(LargeExcelDbContext context, IReportService reportService)
    {
        _context = context;
        _reportService = reportService;
    }

    [HttpPost("SeedData")]
    public async Task<IActionResult> SeedDataAsync()
    {
        await FakeDataHelper.CreateTestDataAsync(_context);

        return Ok();
    }

    [HttpPost("ReportInvoiceExcelWithNpoi")]
    public async Task<IActionResult> ReportInvoiceExcelWithNpoiAsync([FromBody] InvoiceReportRequestModel requestModel)
    {
        var result = await _reportService.InvoiceExcelReportWithNpoiAsync(requestModel);
        return File(result.Bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.Filename);
    }
}