using TestingCRUD.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra;
public class Context : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public Context(DbContextOptions<Context> options)
        : base(options)
    {

    }
}
