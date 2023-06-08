using MediatR;

using TestingCRUD.Application.Dtos;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.Queries.OrderQueries;

namespace TestingCRUD.Application.Handlers.OrderHandlers;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, IEnumerable<OrderViewModel>>
{
    private readonly IOrderReadRepository _orderReadRepository;

    public GetOrdersQueryHandler(IOrderReadRepository orderReadRepository)
    {
        _orderReadRepository = orderReadRepository;
    }

    public async Task<IEnumerable<OrderViewModel>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderReadRepository.GetAll(cancellationToken);

        if (orders is null)
            return Enumerable.Empty<OrderViewModel>();

        var ordersVM = orders.Select(order => new OrderViewModel
        {
            OrderId = order.Id,
            Cpf = order.Cpf,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitValue = item.UnitValue
            }).ToList(),
        });

        return ordersVM;
    }
}
