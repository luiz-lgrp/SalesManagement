using MediatR;

using TestingCRUD.Infra.Repositories;
using TestingCRUD.Application.Queries.ProductQueries;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Infra.QueryHandlers.ProductsQueryHandlers;
public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductViewModel>>
{
    private readonly ProductReadRepository _productReadRepository;

    public GetAllProductsQueryHandler(ProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
    }

    public async Task<IEnumerable<ProductViewModel>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productReadRepository.GetAll(cancellationToken);

        if (products is null)
            return Enumerable.Empty<ProductViewModel>();

        var productsVM = products.Select(product => new ProductViewModel
        {
            Id = product.Id,
            ProductName = product.ProductName,
            Stock = product.Stock,
            Price = product.Price,
            Status = product.Status
        });

        return productsVM;
    }
}
