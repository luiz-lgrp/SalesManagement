using MediatR;

using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Commands.OrderItemCommands;

public class AddItemOnOrderCommand : IRequest<OrderViewModel>
{
    public Guid OrderId { get; set; }
    public OrderItemInputModel NewItem { get; set; }

    public AddItemOnOrderCommand(Guid orderId, OrderItemInputModel newItem)
    {
        OrderId = orderId;
        NewItem = newItem;
    }
}
