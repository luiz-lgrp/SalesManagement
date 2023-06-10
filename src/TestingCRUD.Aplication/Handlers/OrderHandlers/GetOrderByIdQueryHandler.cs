using MediatR;

using TestingCRUD.Application.Dtos;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.Queries.OrderQueries;

namespace TestingCRUD.Application.Handlers.OrderHandlers;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderViewModel>
{
    private readonly IOrderReadRepository _orderReadRepository;

    public GetOrderByIdQueryHandler(IOrderReadRepository orderReadRepository)
    {
        _orderReadRepository = orderReadRepository;
    }

    public async Task<OrderViewModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _orderReadRepository.GetById(request.OrderId, cancellationToken);

        if (order is null)
            return null;

        var orderVM = new OrderViewModel
        {
            OrderId = order.Id,
            Cpf = order.Cpf,
            OrderCode = order.OrderCode,
            TotalValue = order.TotalValue,
            Status = order.Status,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitValue = item.UnitValue
            }).ToList(),
        };

        return orderVM; 
    }
}
