using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.ProductCommands;

namespace TestingCRUD.Application.Handlers.ProductHandlers;
public class DecrementStockCommandHandler : IRequestHandler<DecrementStockCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;

    public DecrementStockCommandHandler(
        IProductRepository productRepository,
        IProductReadRepository productReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<bool> Handle(DecrementStockCommand request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(request.ProductId, cancellationToken);

        if (product is null)
            return false;

        product.DecrementStock(request.Quantity);

        await _productRepository.SaveChangesAsync();

        return true;
    }
}
