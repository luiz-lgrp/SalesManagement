using MediatR;

using TestingCRUD.Application.DTOs;

namespace TestingCRUD.Application.Commands.OrderItemCommands;

public class UpdateQuantityItemCommand : IRequest<bool>
{
    public Guid OrderId { get; }
    public ChangeQuantityItemDTO Model { get; }

    public UpdateQuantityItemCommand(Guid orderId, ChangeQuantityItemDTO model)
    {
        OrderId = orderId;
        Model = model;
    }
}
