using MediatR;

namespace TestingCRUD.Application.Commands.OrderItemCommands;
public class RemoveItemCommand : IRequest<bool>
{
    public Guid OrderId { get; }
    public Guid ProductId { get; }

    public RemoveItemCommand(Guid orderId, Guid productId)
    {
        OrderId = orderId;
        ProductId = productId;
    }
}
