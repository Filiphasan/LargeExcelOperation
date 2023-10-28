using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LargeExcelOperation.Data.EntityConfigurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .HasColumnName("Id")
            .HasColumnType("int")
            .UseIdentityColumn();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("Name")
            .HasColumnType("varchar(80)");

        builder.Property(c => c.Surname)
            .IsRequired()
            .HasMaxLength(80)
            .HasColumnName("Surname")
            .HasColumnType("varchar(80)");

        builder.Property(c => c.Gsm)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnName("Gsm")
            .HasColumnType("varchar(20)");

        builder.Property(c => c.Address)
            .IsRequired()
            .HasMaxLength(360)
            .HasColumnName("Address")
            .HasColumnType("varchar(360)");

        builder.HasIndex(c => c.Gsm)
            .IsUnique()
            .HasDatabaseName("IX_Customers_Gsm");
    }
}