using Microsoft.EntityFrameworkCore;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Infra.Repositories;
public class OrderRepository : IOrderRepository
{
    private readonly Context _context;

    public OrderRepository(Context context) => _context = context;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var orderDelete = await _context.Orders.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (orderDelete is null)
            return false;

        _context.Orders.Remove(orderDelete);

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
