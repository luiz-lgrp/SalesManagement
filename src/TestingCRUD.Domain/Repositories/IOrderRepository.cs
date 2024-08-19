using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories;
public interface IOrderRepository
{
    Task SaveChangesAsync();
    Task<Order> CreateAsync(Order order, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
