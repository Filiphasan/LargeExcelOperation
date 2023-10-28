using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LargeExcelOperation.Data.EntityConfigurations;

public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
{
    public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
    {
        builder.ToTable("InvoiceDetails");

        builder.HasKey(id => id.Id);
        builder.Property(id => id.Id)
            .HasColumnName("Id")
            .HasColumnType("bigint")
            .UseIdentityColumn();

        builder.Property(id => id.InvoiceId)
            .IsRequired()
            .HasColumnName("InvoiceId")
            .HasColumnType("int");

        builder.Property(id => id.OrderDetailId)
            .IsRequired()
            .HasColumnName("OrderDetailId")
            .HasColumnType("bigint");

        builder.Property(id => id.Price)
            .IsRequired()
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)");

        builder.Property(id => id.TaxPrice)
            .IsRequired()
            .HasColumnName("TaxPrice")
            .HasColumnType("decimal(18,2)");

        builder.Property(id => id.TotalPrice)
            .IsRequired()
            .HasColumnName("TotalPrice")
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(id => id.InvoiceId)
            .HasDatabaseName("IX_InvoiceDetails_InvoiceId");

        builder.HasIndex(id => id.OrderDetailId)
            .HasDatabaseName("IX_InvoiceDetails_OrderDetailId");

        builder.HasOne(id => id.Invoice)
            .WithMany(i => i.InvoiceDetails)
            .HasForeignKey(id => id.InvoiceId);

        builder.HasOne(id => id.OrderDetail)
            .WithMany()
            .HasForeignKey(id => id.OrderDetailId);
    }
}