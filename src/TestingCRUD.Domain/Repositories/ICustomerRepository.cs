using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken);
        Task<Customer> UpdateAsync(string cpf, Customer customer, CancellationToken cancellationToken);
        Task<Customer> DeleteAsync(string cpf, CancellationToken cancellationToken);
    }
}
