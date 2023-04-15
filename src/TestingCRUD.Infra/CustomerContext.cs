using TestingCRUD.Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra
{
    public class CustomerContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {

        }
    }
}
