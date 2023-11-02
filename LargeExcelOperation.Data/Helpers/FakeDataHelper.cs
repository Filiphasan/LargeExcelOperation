using Bogus;
using Bogus.Extensions;
using LargeExcelOperation.Data.Contexts;
using LargeExcelOperation.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LargeExcelOperation.Data.Helpers;

public static class FakeDataHelper
{
    public static async Task CreateTestDataAsync(LargeExcelDbContext context)
    {
        context.Database.SetCommandTimeout(TimeSpan.FromMinutes(10));

        var categories = CreateCategories();
        if (await context.Categories.AnyAsync())
        {
            categories = await context.Categories.ToListAsync();
        }
        else
        {
            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();
        }
        

        var products = CreateProducts(categories);
        if (await context.Products.AnyAsync())
        {
            products = await context.Products.ToListAsync();
        }
        else
        {
            await context.Products.AddRangeAsync(products);
            await context.SaveChangesAsync();
        }
        
        var customers = CreateCustomers();
        if (await context.Customers.AnyAsync())
        {
            customers = await context.Customers.ToListAsync();
        }
        else
        {
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        var orders = CreateOrders(products, customers);
        if (await context.Orders.AnyAsync())
        {
            orders = await context.Orders.ToListAsync();
        }
        else
        {
            await context.Orders.AddRangeAsync(orders);
            await context.SaveChangesAsync();
        }

        var orderDetails = CreateOrderDetails(orders, products);
        if (await context.OrderDetails.AnyAsync())
        {
            orderDetails = await context.OrderDetails.ToListAsync();
        }
        else
        {
            await context.OrderDetails.AddRangeAsync(orderDetails);
            await context.SaveChangesAsync();
        }

        var invoices = CreateInvoices(orders);
        if (await context.Invoices.AnyAsync())
        {
            invoices = await context.Invoices.ToListAsync();
        }
        else
        {
            await context.Invoices.AddRangeAsync(invoices);
            await context.SaveChangesAsync();
        }

        var invoiceDetails = CreateInvoiceDetails(invoices, orderDetails);
        if (!await context.InvoiceDetails.AnyAsync())
        {
            await context.InvoiceDetails.AddRangeAsync(invoiceDetails);
            await context.SaveChangesAsync();
        }
        else
        {
            invoiceDetails = await context.InvoiceDetails.ToListAsync();
        }

        if (invoices.Any(x => x.Price == 0))
        {
            await FillInvoicePriceFieldsAsync(context);
        }
    }

    private static IEnumerable<Category> CreateCategories()
    {
        var faker = new Faker<Category>();

        var data = faker
            .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(180).ClampLength(400, 1000))
            .Generate(100);

        data = data.DistinctBy(x => x.Name).Take(100).ToList();

        return data;
    }

    private static IEnumerable<Product> CreateProducts(IEnumerable<Category> categories)
    {
        var faker = new Faker<Product>();
        
        var data = faker
            .RuleFor(p => p.Name, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence(180).ClampLength(400, 1000))
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000))
            .RuleFor(p => p.Barcode, f => f.Random.Long(1_000_000_000_000, 9_999_999_999_999))
            .RuleFor(p => p.StockQuantity, f => f.Random.Int(1, 100))
            .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
            .Generate(1_000);
        
        return data;
    }

    private static IEnumerable<Customer> CreateCustomers()
    {
        var faker = new Faker<Customer>();

        var data = faker
            .RuleFor(c => c.Name, f => f.Person.FirstName)
            .RuleFor(c => c.Surname, f => f.Person.LastName)
            .RuleFor(c => c.Gsm, f => $"05{f.Random.Number(100_000_000, 999_999_999)}")
            .RuleFor(c => c.Address, f => f.Address.FullAddress())
            .Generate(200);
        
        return data;
    }

    private static IEnumerable<Order> CreateOrders(IEnumerable<Product> products, IEnumerable<Customer> customers)
    {
        var faker = new Faker<Order>();
        
        var data = faker
            .RuleFor(o => o.OrderNumber, f => f.Random.Long(1_000_000_000_000, 9_999_999_999_999))
            .RuleFor(o => o.CustomerId, f => f.PickRandom(customers).Id)
            .RuleFor(o => o.OrderDate, f => f.Date.Between(DateTime.Today.AddYears(-1), DateTime.Today.AddDays(1)))
            .Generate(4_000);

        return data;
    }

    private static IEnumerable<OrderDetail> CreateOrderDetails(IEnumerable<Order> orders, IEnumerable<Product> products)
    {
        var faker = new Faker<OrderDetail>();

        var data = faker
            .RuleFor(o => o.OrderId, f => f.PickRandom(orders).Id)
            .RuleFor(o => o.Product, f => f.PickRandom(products))
            .RuleFor(o => o.ProductId, (_,o) => o.Product!.Id)
            .RuleFor(o => o.Quantity, f => f.Random.Number(1, 20))
            .RuleFor(o => o.Price, (f,o) => o.Quantity * o.Product!.Price)
            .RuleFor(o => o.TaxPrice, (_,o) => o.Price * (decimal)0.18)
            .RuleFor(o => o.TotalPrice, (_,o) => o.Price + o.TaxPrice)
            .Generate(100_000);
        
        return data;
    }

    private static IEnumerable<Invoice> CreateInvoices(IEnumerable<Order> orders)
    {
        var faker = new Faker();

        var data = orders.Select(x => new Invoice()
        {
            InvoiceDate = x.OrderDate.AddDays(4),
            EInvoiceNumber = $"TR{faker.Random.Long(1_000_000_000_000, 9_999_999_999_999)}",
            EInvoiceDate = x.OrderDate.AddDays(4),
            CustomerId = x.CustomerId,
            OrderId = x.Id,
            Price = 0,
            TaxPrice = 0,
            TotalPrice = 0,
        });
        
        return data;
    }

    private static IEnumerable<InvoiceDetail> CreateInvoiceDetails(IEnumerable<Invoice> invoices, IEnumerable<OrderDetail> orderDetails)
    {
        var data = orderDetails.Select(x => new InvoiceDetail()
        {
            InvoiceId = invoices.First(i => i.OrderId == x.OrderId).Id,
            OrderDetailId = x.Id,
            Price = x.Price,
            TaxPrice = x.TaxPrice,
            TotalPrice = x.TotalPrice,
        });

        return data;
    }

    private static async Task FillInvoicePriceFieldsAsync(LargeExcelDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(@"
                        UPDATE Invoices SET Price = ISNULL((SELECT SUM(Price) FROM InvoiceDetails WHERE InvoiceDetails.InvoiceId = Invoices.Id),0)
                        UPDATE Invoices SET TaxPrice  = ISNULL((SELECT SUM(ID.TaxPrice) FROM InvoiceDetails ID WHERE ID.InvoiceId = Invoices.Id), 0)
                        UPDATE Invoices SET TotalPrice = ISNULL((SELECT SUM(ID.TotalPrice) FROM InvoiceDetails ID WHERE ID.InvoiceId = Invoices.Id),0)
                        ");
    }
}