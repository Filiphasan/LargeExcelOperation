using LargeExcelOperation.Data.Contexts;
using LargeExcelOperation.Data.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace LargeExcelOperation.Rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExcelController : ControllerBase
{
    private readonly LargeExcelDbContext _context;

    public ExcelController(LargeExcelDbContext context)
    {
        _context = context;
    }

    [HttpPost("SeedData")]
    public async Task<IActionResult> SeedDataAsync()
    {
        await FakeDataHelper.CreateTestDataAsync(_context);

        return Ok();
    }
}