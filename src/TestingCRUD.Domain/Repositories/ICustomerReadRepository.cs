using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories
{
    public interface ICustomerReadRepository
    {
        Task<IEnumerable<Customer>> GetAll(CancellationToken cancellationToken);
        Task<Customer> GetByCpf(string cpf, CancellationToken cancellationToken);
    }
}
