using MediatR;

using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Application.Queries.ProductQueries;

public class GetProductByIdQuery : IRequest<ProductViewModel>
{
    public Guid Id { get; set; }

    public GetProductByIdQuery(Guid id) => Id = id;
}
