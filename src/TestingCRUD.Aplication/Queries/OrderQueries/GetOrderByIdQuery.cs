using MediatR;
using TestingCRUD.Application.ViewModels;

namespace TestingCRUD.Application.Queries.OrderQueries;
public class GetOrderByIdQuery :IRequest<OrderViewModel>
{
    public Guid OrderId { get; set; }

    public GetOrderByIdQuery(Guid id) => OrderId = id;
}
