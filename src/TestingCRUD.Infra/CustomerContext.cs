using TestingCRUD.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {

        }
    }
}
