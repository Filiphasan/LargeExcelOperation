namespace LargeExcelOperation.Data.Entities;

public class Invoice
{
    public int Id { get; set; }
    public DateTime InvoiceDate { get; set; }
    public string EInvoiceNumber { get; set; }
    public DateTime EInvoiceDate { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public decimal TaxPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public Customer? Customer { get; set; }
    public Order? Order { get; set; }
    public ICollection<InvoiceDetail> InvoiceDetails { get; set; }
}