using FluentValidation;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Validations.ProductCommandValidation;
public class CreateProductValidator : AbstractValidator<ProductInputModel>
{
    private readonly IProductReadRepository? _productReadRepository;

    public CreateProductValidator(IProductReadRepository? productReadRepository)
    {
        _productReadRepository = productReadRepository;

        RuleFor(p => p.ProductName)
            .NotEmpty().WithMessage("Digite o nome do produto")
            .MaximumLength(20).WithMessage("O campo nome não pode passar de 20 caracteres")
            .MinimumLength(3).WithMessage("O campo nome não pode ser menor que 03 caracteres");

        RuleFor(p => p.ProductName)
            .MustAsync(NameIsValid).WithMessage("Já existe um produto com este nome");

        RuleFor(p => p.Stock)
            .NotEmpty().WithMessage("Digite a quantidade em estoque")
            .GreaterThan(0).WithMessage("O valor em estoque deve ser maior que zero")
            .Must(IsInteger).WithMessage("O valor em estoque deve ser um número inteiro");

        RuleFor(p => p.Price)
            .NotEmpty().WithMessage("Digite o valor do produto")
            .GreaterThan(0).WithMessage("O valor em estoque deve ser maior que zero");
    }

    private bool IsInteger(int value) => value % 1 == 0;

    private async Task<bool> NameIsValid(string productName, CancellationToken cancellationToken)
    {
        var product = await _productReadRepository.GetByName(productName, cancellationToken);

        return product is null;
    }
}
