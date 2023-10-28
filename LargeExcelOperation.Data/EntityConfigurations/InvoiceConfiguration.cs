using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LargeExcelOperation.Data.EntityConfigurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.ToTable("Invoices");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("Id")
            .HasColumnType("int")
            .UseIdentityColumn();

        builder.Property(i => i.InvoiceDate)
            .IsRequired()
            .HasColumnName("InvoiceDate")
            .HasColumnType("datetime2");

        builder.Property(i => i.EInvoiceNumber)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("EInvoiceNumber")
            .HasColumnType("varchar(50)");

        builder.Property(i => i.EInvoiceDate)
            .IsRequired()
            .HasColumnName("EInvoiceDate")
            .HasColumnType("datetime2");

        builder.Property(i => i.CustomerId)
            .IsRequired()
            .HasColumnName("CustomerId")
            .HasColumnType("int");

        builder.Property(i => i.OrderId)
            .IsRequired()
            .HasColumnName("OrderId")
            .HasColumnType("int");

        builder.Property(i => i.Price)
            .IsRequired()
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.TaxPrice)
            .IsRequired()
            .HasColumnName("TaxPrice")
            .HasColumnType("decimal(18,2)");

        builder.Property(i => i.TotalPrice)
            .IsRequired()
            .HasColumnName("TotalPrice")
            .HasColumnType("decimal(18,2)");

        builder.HasIndex(i => i.EInvoiceNumber)
            .HasDatabaseName("IX_Invoices_EInvoiceNumber");

        builder.HasIndex(i => i.CustomerId)
            .HasDatabaseName("IX_Invoices_CustomerId");

        builder.HasIndex(i => i.OrderId)
            .HasDatabaseName("IX_Invoices_OrderId");

        builder.HasMany(i => i.InvoiceDetails)
            .WithOne(id => id.Invoice)
            .HasForeignKey(id => id.InvoiceId);
    }
}