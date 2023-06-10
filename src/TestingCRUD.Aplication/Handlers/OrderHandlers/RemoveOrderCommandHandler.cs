using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderCommands;

namespace TestingCRUD.Application.Handlers.OrderHandlers;

public class RemoveOrderCommandHandler : IRequestHandler<RemoveOrderCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public RemoveOrderCommandHandler(
        IOrderRepository orderRepository, 
        IOrderReadRepository orderReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<bool> Handle(RemoveOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository.GetById(request.OrderId, cancellationToken);

        if (order is null)
            return false;

        var OrderRemoved = await _orderRepository.DeleteAsync(order.Id, cancellationToken);

        return OrderRemoved;
    }
}
