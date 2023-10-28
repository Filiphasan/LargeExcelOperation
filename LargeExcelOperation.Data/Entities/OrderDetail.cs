namespace LargeExcelOperation.Data.Entities;

public class OrderDetail
{
    public long Id { get; set; }
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TaxPrice { get; set; }
    public decimal TotalPrice { get; set; }
    public Product? Product { get; set; }
    public Order? Order { get; set; }
}