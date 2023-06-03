using MediatR;

using TestingCRUD.Domain.Enums;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.ProductCommands;

namespace TestingCRUD.Application.Handlers.ProductHandlers;
public class InactivateProductCommandHandler : IRequestHandler<InactivateProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductReadRepository _productReadRepository;

    public InactivateProductCommandHandler(
        IProductRepository productRepository, 
        IProductReadRepository productReadRepository)
    {
        _productRepository = productRepository;
        _productReadRepository = productReadRepository;
    }

    public async Task<bool> Handle(InactivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(request.ProductId, cancellationToken);

        if (product is null)
            return false;

        product.Inactive();

        await _productRepository.SaveChangesAsync();

        return true;
    }
}
