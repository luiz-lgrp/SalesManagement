using MediatR;

namespace TestingCRUD.Application.Commands.OrderCommands;

public class RemoveOrderCommand : IRequest<bool>
{
    public Guid OrderId { get; set; }

    public RemoveOrderCommand(Guid id) => OrderId = id;
}
