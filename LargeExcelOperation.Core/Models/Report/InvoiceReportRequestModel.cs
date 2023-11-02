namespace LargeExcelOperation.Core.Models.Report;

public struct InvoiceReportRequestModel
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class InvoiceReportResultModel
{
    public byte[] Bytes { get; set; }
    public string? Filename { get; set; }

    public InvoiceReportResultModel()
    {
        Bytes = Array.Empty<byte>();
    }
}