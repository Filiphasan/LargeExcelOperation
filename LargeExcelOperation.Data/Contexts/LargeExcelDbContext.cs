using System.Reflection;
using LargeExcelOperation.Data.Entities;
using LargeExcelOperation.Data.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LargeExcelOperation.Data.Contexts;

public class LargeExcelDbContext : DbContext
{
    public LargeExcelDbContext(DbContextOptions<LargeExcelDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public DbSet<Invoice> Invoices { get; set; }
    public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(CategoryConfiguration)) ?? throw new InvalidOperationException());
        base.OnModelCreating(modelBuilder);
    }
}