using MediatR;

namespace TestingCRUD.Application.Commands.OrderCommands;

public class ExchangeStatusAwaitingPaymentCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }

    public ExchangeStatusAwaitingPaymentCommand(Guid id) => OrderId = id;
}
