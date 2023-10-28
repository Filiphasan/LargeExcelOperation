namespace LargeExcelOperation.Data.Entities;

public class InvoiceDetail
{
    public long Id { get; set; }
    public int InvoiceId { get; set; }
    public long OrderDetailId { get; set; }
    public decimal Price { get; set; }
    public decimal TaxPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public Invoice Invoice { get; set; }
    public OrderDetail OrderDetail { get; set; }
}