using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;
public class OrderReadRepository : IOrderReadRepository
{
    private readonly Context _orderContext;

    public OrderReadRepository(Context orderContext) => _orderContext = orderContext;

    public async Task<IEnumerable<Order>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderContext.Orders
            .Include(o => o.OrderItems)
            .ToListAsync(cancellationToken);

        if (!orders.Any())
            return Enumerable.Empty<Order>();
        
        return orders;
    }

    public async Task<Order> GetById(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

        if (order is null)
            return null;

        return order;
    }
}
