using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly Context _context;

    public CustomerRepository(Context context) => _context = context;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<bool> DeleteAsync(string cpf, CancellationToken cancellationToken)
    {
        var customerDelete = await _context.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);

        if (customerDelete is null)
            return false;

        _context.Customers.Remove(customerDelete);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> UpdateAsync(string cpf, Customer customer, CancellationToken cancellationToken)
    {
        var findedCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf, cancellationToken);

        if (findedCustomer is null)
            return false;

        findedCustomer.Name = customer.Name;
        findedCustomer.Cpf = customer.Cpf;
        findedCustomer.Email = customer.Email;
        findedCustomer.Phone = customer.Phone;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
