using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.ProductCommands;
using TestingCRUD.Application.ViewModels.ProductViewModels;
using TestingCRUD.Application.Validations.ProductCommandValidation;

namespace TestingCRUD.Application.Handlers.ProductHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createModel = request.ProductModel;

        var validationResult = new CreateProductValidator().Validate(createModel);
        
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var product = new Product(
            createModel.ProductName,
            createModel.Stock,
            createModel.Price);

        var createdProduct = await _productRepository.CreateAsync(product, cancellationToken);

        if (createdProduct is null)
            return null;

        var productVM = new ProductViewModel
        {
            Id = createdProduct.Id,
            ProductName = createdProduct.ProductName,
            Stock = createdProduct.Stock,
            Price = createdProduct.Price,
            Status = createdProduct.Status
        };

        return productVM;
    }
}
