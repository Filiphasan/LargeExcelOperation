using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LargeExcelOperation.Data.EntityConfigurations;

public class OrderDetailsConfiguration : IEntityTypeConfiguration<OrderDetail>
{
    public void Configure(EntityTypeBuilder<OrderDetail> builder)
    {
        builder.ToTable("OrderDetails");
        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrderId)
            .IsRequired()
            .HasColumnName("OrderId");
        builder.Property(o => o.ProductId)
            .IsRequired()
            .HasColumnName("ProductId");
        builder.Property(o => o.Quantity)
            .IsRequired()
            .HasColumnName("Quantity");
        builder.Property(o => o.Price)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasColumnName("Price");
        builder.Property(o => o.TaxPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasColumnName("TaxPrice");
        builder.Property(o => o.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasColumnName("TotalPrice");

        builder.HasOne(o => o.Order)
            .WithMany(order => order.OrderDetails)
            .HasForeignKey(o => o.OrderId);

        builder.HasIndex(o => o.OrderId)
            .HasDatabaseName("IX_OrderDetails_OrderId");
    }
}