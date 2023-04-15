using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra.Repositories
{
    public class CustomerReadRepository : ICustomerReadRepository
    {
        private readonly CustomerContext _customerContext;

        public CustomerReadRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task<IEnumerable<Customer>> GetAll(CancellationToken cancellationToken)
        {
            var customers = await _customerContext.Customers.ToListAsync();

            return customers;
        }

        public async Task<Customer> GetByCpf(string cpf, CancellationToken cancellationToken)
        {
            var customer = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf);

            return customer;
        }
    }
}
