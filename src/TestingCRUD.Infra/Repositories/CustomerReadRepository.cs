using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;
public class CustomerReadRepository : ICustomerReadRepository
{
    private readonly Context _customerContext;

    public CustomerReadRepository(Context customerContext)
    {
        _customerContext = customerContext;
    }

    public async Task<IEnumerable<Customer>> GetAll(CancellationToken cancellationToken)
    {
        var customers = await _customerContext.Customers.ToListAsync();

        if (!customers.Any())
            return Enumerable.Empty<Customer>();

        return customers;
    }

    public async Task<Customer?> GetByCpf(string cpf, CancellationToken cancellationToken)
    {
        var customer = await _customerContext.Customers.FirstOrDefaultAsync(c => c.Cpf == cpf);

        if (customer == null) 
            return null;

        return customer;
    }
}
