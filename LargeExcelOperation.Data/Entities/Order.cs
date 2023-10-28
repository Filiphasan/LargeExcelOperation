namespace LargeExcelOperation.Data.Entities;

public class Order
{
    public int Id { get; set; }
    public long OrderNumber { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public Customer? Customer { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
}