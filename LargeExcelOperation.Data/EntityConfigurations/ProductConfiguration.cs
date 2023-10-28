using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LargeExcelOperation.Data.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnName("Id").HasColumnType("int").UseIdentityColumn();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(240).HasColumnName("Name").HasColumnType("varchar(240)");
        builder.Property(p => p.Description).IsRequired().HasMaxLength(2000).HasColumnName("Description").HasColumnType("varchar(2000)");
        builder.Property(p => p.Price).IsRequired().HasColumnName("Price").HasColumnType("decimal(18,2)");
        builder.Property(p => p.Barcode).IsRequired().HasColumnName("Barcode").HasColumnType("bigint");
        builder.Property(p => p.StockQuantity).IsRequired().HasColumnName("StockQuantity").HasColumnType("int");

        builder.HasIndex(p => p.Name).HasDatabaseName("IX_Products_Name");
        builder.HasIndex(p => p.Barcode).HasDatabaseName("IX_Products_Barcode");

        builder.HasOne(p => p.Category)
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}