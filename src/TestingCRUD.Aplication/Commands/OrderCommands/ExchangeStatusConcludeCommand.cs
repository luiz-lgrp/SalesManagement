using MediatR;

namespace TestingCRUD.Application.Commands.OrderCommands;

public class ExchangeStatusConcludeCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }

    public ExchangeStatusConcludeCommand(Guid id) => OrderId = id;
}
