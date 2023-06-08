using MediatR;
using TestingCRUD.Application.ViewModels;

namespace TestingCRUD.Application.Queries.OrderQueries;
public class GetOrdersQuery : IRequest<IEnumerable<OrderViewModel>>
{

}
