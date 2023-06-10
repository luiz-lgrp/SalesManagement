using TestingCRUD.Domain.Models;

namespace TestingCRUD.Domain.Repositories;
public interface IOrderReadRepository 
{
    Task<IEnumerable<Order>> GetAll(CancellationToken cancellationToken);
    Task<Order?> GetById(Guid id, CancellationToken cancellationToken);
}
