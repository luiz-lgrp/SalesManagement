using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Infra.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _customerContext;

        public CustomerRepository(CustomerContext customerContext)
        {
            _customerContext = customerContext;
        }

        public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken)
        {
            _customerContext.Customers.AddAsync(customer, cancellationToken);
            await _customerContext.SaveChangesAsync();

            return customer;
        }

        public async Task<Customer> DeleteAsync(string cpf, CancellationToken cancellationToken)
        {
            var customerDelete = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf);

            if (customerDelete is null)
                return null;

            _customerContext.Remove(customerDelete);

            _customerContext.SaveChanges();

            return customerDelete;
        }

        public async Task<Customer> UpdateAsync(string cpf, Customer customer, CancellationToken cancellationToken)
        {
            var findedCustomer = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);

            if (findedCustomer is null)
                return null;

            findedCustomer.Name = customer.Name;
            findedCustomer.Cpf = customer.Cpf;
            findedCustomer.Email = customer.Email;
            findedCustomer.Phone = customer.Phone;

            await _customerContext.SaveChangesAsync(cancellationToken);

            return findedCustomer;
        }
    }
}
