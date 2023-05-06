using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories
{
    public interface ICustomerRepository
    {
        Task SaveChangesAsync();
        Task<Customer> CreateAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> UpdateAsync(string cpf, Customer customer, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(string cpf, CancellationToken cancellationToken);
    }
}
