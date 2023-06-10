using MediatR;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Commands.OrderCommands;
public class CreateOrderCommand : IRequest<OrderViewModel>
{
    public OrderInputModel Order { get; set; }

    public CreateOrderCommand(OrderInputModel order) => Order = order;
}
