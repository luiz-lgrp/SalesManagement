using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderItemCommands;

namespace TestingCRUD.Application.Handlers.OrderItemHandlers;

public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public RemoveItemCommandHandler(
        IOrderRepository orderRepository,
        IOrderReadRepository orderReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<bool> Handle(RemoveItemCommand request, CancellationToken cancellationToken)
    {
        var orderId = request.OrderId;
        var productId = request.ProductId;

        var order = await _orderReadRepository.GetById(orderId, cancellationToken);

        if (order is null)
            return false;

        var itemToRemove = order.OrderItems.FirstOrDefault(item => item.ProductId == productId);

        if (itemToRemove is null)
            return false;

        order.RemoveItem(itemToRemove);
        
        await _orderRepository.SaveChangesAsync();

        return true;
    }
}
