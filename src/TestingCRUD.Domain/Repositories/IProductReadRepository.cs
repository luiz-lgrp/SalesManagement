using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories;
public interface IProductReadRepository
{
    Task<IEnumerable<Product>> GetAll(CancellationToken cancellationToken);
    Task<Product?> GetById(Guid id, CancellationToken cancellationToken);
}
