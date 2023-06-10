using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderItemCommands;

namespace TestingCRUD.Application.Handlers.OrderItemHandlers;
public class UpdateQuantityItemCommandHandler : IRequestHandler<UpdateQuantityItemCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public UpdateQuantityItemCommandHandler(
        IOrderRepository orderRepository, 
        IOrderReadRepository orderReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<bool> Handle(UpdateQuantityItemCommand request, CancellationToken cancellationToken)
    {
        var orderId = request.OrderId;
        var productId = request.Model.ProductId;
        var newQuatity = request.Model.NewQuantity;

        var order = await _orderReadRepository.GetById(orderId, cancellationToken);

        if (order is null)
            return false;

        var itemToUpdate = order.OrderItems.FirstOrDefault(item => item.ProductId == productId);

        if (itemToUpdate is null)
            return false;

        order.UpdateQuantityItem(itemToUpdate, newQuatity);

        await _orderRepository.SaveChangesAsync();

        return true;
    }
}
