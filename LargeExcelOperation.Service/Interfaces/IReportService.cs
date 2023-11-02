using LargeExcelOperation.Core.Models;
using LargeExcelOperation.Core.Models.Report;

namespace LargeExcelOperation.Service.Interfaces;

public interface IReportService
{
    Task<InvoiceReportResultModel> InvoiceExcelReportWithNpoiAsync(InvoiceReportRequestModel requestModel);
}