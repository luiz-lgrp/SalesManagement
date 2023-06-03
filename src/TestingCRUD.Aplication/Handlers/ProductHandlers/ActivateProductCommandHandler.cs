using MediatR;

using TestingCRUD.Application.Commands.ProductCommands;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Handlers.ProductHandlers;
public class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;

    public ActivateProductCommandHandler(
        IProductRepository productRepository, 
        IProductReadRepository productReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<bool> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(request.ProductId, cancellationToken);

        if (product is null)
            return false;

        product.Active();

        await _productRepository.SaveChangesAsync();

        return true;
    }
}
