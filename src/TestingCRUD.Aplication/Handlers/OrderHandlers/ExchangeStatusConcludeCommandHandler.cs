using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderCommands;

namespace TestingCRUD.Application.Handlers.OrderHandlers;

public class ExchangeStatusConcludeCommandHandler : IRequestHandler<ExchangeStatusConcludeCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public ExchangeStatusConcludeCommandHandler(
        IOrderRepository orderRepository, 
        IOrderReadRepository orderReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<bool> Handle(ExchangeStatusConcludeCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository.GetById(request.OrderId, cancellationToken);

        if (order is null)
            return false;

        order.Conclude();

        await _orderRepository.SaveChangesAsync();

        return true;
    }
}
