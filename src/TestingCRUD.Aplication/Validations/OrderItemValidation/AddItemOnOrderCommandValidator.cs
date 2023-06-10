using FluentValidation;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Validations.OrderItemValidation;
public class AddItemOnOrderCommandValidator : AbstractValidator<OrderItemInputModel>
{
    private readonly IProductReadRepository _productReadRepository;

    public AddItemOnOrderCommandValidator(IProductReadRepository productReadRepository)
    {
        _productReadRepository = productReadRepository;
        
        RuleFor(p => p.ProductId)
            .MustAsync(ProductIdIsValid).WithMessage("Não existe um produto com este Id");

        RuleFor(p => p.ProductName)
            .MustAsync(NameIsValid).WithMessage("Não existe um produto com este nome");
    }

    private async Task<bool> NameIsValid(string productName, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByName(productName, cancellationToken);

        return product is not null;
    }

    private async Task<bool> ProductIdIsValid(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetById(id, cancellationToken);

        return product is not null;
    }
}
