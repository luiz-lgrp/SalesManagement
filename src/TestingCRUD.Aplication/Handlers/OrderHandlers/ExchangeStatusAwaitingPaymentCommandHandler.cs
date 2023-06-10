using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderCommands;

namespace TestingCRUD.Application.Handlers.OrderHandlers;

public class ExchangeStatusAwaitingPaymentCommandHandler : IRequestHandler<ExchangeStatusAwaitingPaymentCommand, bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;

    public ExchangeStatusAwaitingPaymentCommandHandler(
        IOrderRepository orderRepository, 
        IOrderReadRepository orderReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
    }

    public async Task<bool> Handle(ExchangeStatusAwaitingPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository.GetById(request.OrderId, cancellationToken);

        if (order is null)
            return false;

        order.WaitForPayment();

        await _orderRepository.SaveChangesAsync();

        return true;
    }
}
