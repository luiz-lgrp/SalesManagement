using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories;
public interface IProductRepository
{
    Task SaveChangesAsync();
    Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
