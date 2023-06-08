using MediatR;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Application.Queries.ProductQueries;
public class GetAllProductsQuery : IRequest<IEnumerable<ProductViewModel>>
{

}
