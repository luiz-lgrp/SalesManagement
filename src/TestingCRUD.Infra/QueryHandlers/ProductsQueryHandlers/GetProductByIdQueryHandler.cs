using MediatR;

using TestingCRUD.Infra.Repositories;
using TestingCRUD.Application.Queries.ProductQueries;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Infra.QueryHandlers.ProductsQueryHandlers;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductViewModel>
{
    private readonly ProductReadRepository _productReadRepository;

    public GetProductByIdQueryHandler(ProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<ProductViewModel> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(request.Id, cancellationToken);

        if (product is null)
            return null;

        var productVM = new ProductViewModel
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Stock = product.Stock,
            Price = product.Price,
            Status = product.Status
        };

        return productVM;
    }
}