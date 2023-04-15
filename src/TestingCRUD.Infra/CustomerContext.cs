using TestingCRUD.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
    }
}
