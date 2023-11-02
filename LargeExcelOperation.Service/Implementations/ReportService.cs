using LargeExcelOperation.Core.Helpers.ExtensionHelper;
using LargeExcelOperation.Core.Models;
using LargeExcelOperation.Core.Models.Report;
using LargeExcelOperation.Data.Contexts;
using LargeExcelOperation.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LargeExcelOperation.Service.Implementations;

public class ReportService : IReportService
{
    private readonly LargeExcelDbContext _context;
    private readonly IExcelService _excelService;
    private readonly ILogger<ReportService> _logger;

    public ReportService(LargeExcelDbContext context, IExcelService excelService, ILogger<ReportService> logger)
    {
        _context = context;
        _context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));
        _excelService = excelService;
        _logger = logger;
    }

    private async Task<IList<InvoiceReportSelectModel>> GetDataAsync(InvoiceReportRequestModel requestModel)
    {
        var query = (from invoice in _context.Invoices
                join invoiceDetail in _context.InvoiceDetails on invoice.Id equals invoiceDetail.InvoiceId
                join order in _context.Orders on invoice.OrderId equals order.Id
                join orderDetail in _context.OrderDetails on invoiceDetail.OrderDetailId equals orderDetail.Id
                join product in _context.Products on orderDetail.ProductId equals product.Id
                join category in _context.Categories on product.CategoryId equals category.Id
                join customer in _context.Customers on invoice.CustomerId equals customer.Id
                where invoice.InvoiceDate >= requestModel.StartDate!.Value
                      && invoice.InvoiceDate <= requestModel.EndDate!.Value
                select new
                {
                    InvoiceNo = invoice.Id,
                    InvoiceDate = invoice.InvoiceDate,
                    EInvoiceNumber = invoice.EInvoiceNumber,
                    EInvoiceDate = invoice.EInvoiceDate,
                    CustomerName = customer.Name,
                    CustomerSurname = customer.Surname,
                    CustomerGsm = customer.Gsm,
                    CustomerAddress = customer.Address,
                    OrderNumber = order.OrderNumber,
                    OrderQuantity = orderDetail.Quantity,
                    ProductName = product.Name,
                    ProductDescription = product.Description,
                    ProductPrice = product.Price,
                    ProductBarcode = product.Barcode,
                    ProductCategory = category.Name,
                    Price = invoiceDetail.Price,
                    TaxPrice = invoiceDetail.TaxPrice,
                    TotalPrice = invoiceDetail.TotalPrice,
                    GrandPrice = invoice.Price,
                    GrandTaxPrice = invoice.TaxPrice,
                    GrandTotalPrice = invoice.TotalPrice,
                })
            .AsNoTracking();

        var queryString = query.ToQueryString();

        var data = await query.ToListAsync();

        return data.Select(x => new InvoiceReportSelectModel()
        {
            InvoiceNo = x.InvoiceNo.ToString(),
            InvoiceDate = x.InvoiceDate.ToString("dd-MM-yyyy HH:mm"),
            EInvoiceNumber = x.EInvoiceNumber,
            EInvoiceDate = x.EInvoiceDate.ToString("dd-MM-yyyy HH:mm"),
            CustomerName = x.CustomerName,
            CustomerSurname = x.CustomerSurname,
            CustomerGsm = x.CustomerGsm,
            CustomerAddress = x.CustomerAddress,
            OrderNumber = x.OrderNumber.ToString(),
            OrderQuantity = x.OrderQuantity.ToString(),
            ProductName = x.ProductName,
            ProductDescription = x.ProductDescription,
            ProductPrice = x.ProductPrice.ToCustomCurrency(),
            ProductBarcode = x.ProductBarcode.ToString(),
            ProductCategory = x.ProductCategory,
            Price = x.Price.ToCustomCurrency(),
            TaxPrice = x.TaxPrice.ToCustomCurrency(),
            TotalPrice = x.TotalPrice.ToCustomCurrency(),
            GrandPrice = x.GrandPrice.ToCustomCurrency(),
            GrandTaxPrice = x.GrandTaxPrice.ToCustomCurrency(),
            GrandTotalPrice = x.GrandTotalPrice.ToCustomCurrency(),
        }).ToList();
    }

    public async Task<InvoiceReportResultModel> InvoiceExcelReportWithNpoiAsync(InvoiceReportRequestModel requestModel)
    {
        const string methodName = $"{nameof(ReportService)} - {nameof(InvoiceExcelReportWithNpoiAsync)}";
        try
        {
            var list = await GetDataAsync(requestModel);

            var excelBytes = await _excelService.CreateExcelWithNpoiAsync(list);
            return new InvoiceReportResultModel()
            {
                Bytes = excelBytes,
                Filename = $"Invoice Excel Report With NPOI - {DateTime.Now:dd-MM-yyyy HH:mm:ss}"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{methodName} Exception", methodName);
            return new InvoiceReportResultModel();
        }
    }
}