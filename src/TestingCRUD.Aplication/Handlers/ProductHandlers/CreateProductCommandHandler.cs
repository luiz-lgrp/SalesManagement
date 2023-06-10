using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.ProductCommands;
using TestingCRUD.Application.ViewModels.ProductViewModels;
using TestingCRUD.Application.Validations.ProductCommandValidation;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Handlers.ProductHandlers;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductViewModel>
{
    private readonly IProductRepository _productRepository;
    private readonly IValidator<ProductInputModel> _validator;

    public CreateProductCommandHandler(
        IProductRepository productRepository, 
        IValidator<ProductInputModel> validator)
    {
        _productRepository = productRepository;
        _validator = validator;
    }

    public async Task<ProductViewModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var createModel = request.ProductModel;

        var validationResult = await _validator.ValidateAsync(createModel);
        
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
