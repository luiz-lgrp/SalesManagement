using MediatR;
 
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.ProductCommands;

namespace TestingCRUD.Application.Handlers.ProductHandlers;

public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;

    public RemoveProductCommandHandler(
        IProductRepository productRepository, 
        IProductReadRepository productReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<bool> Handle(RemoveProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(request.Id, cancellationToken);

        if (product is null)
            return false;

        var productDeleted = await _productRepository.DeleteAsync(product.Id, cancellationToken);

        return productDeleted;
    }
}
