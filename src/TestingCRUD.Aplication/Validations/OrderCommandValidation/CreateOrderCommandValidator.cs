using FluentValidation;

using TestingCRUD.Domain.Enums;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Validations.OrderCommandValidation;
public class CreateOrderCommandValidator : AbstractValidator<OrderInputModel>
{
    private readonly ICustomerReadRepository _customerReadRepository;
    private readonly IProductReadRepository _productReadRepository;
    public CreateOrderCommandValidator(
        ICustomerReadRepository customerReadRepository, 
        IProductReadRepository productReadRepository)
    {
        _customerReadRepository = customerReadRepository;
        _productReadRepository = productReadRepository;

        RuleFor(o => o.Cpf)
            .NotEmpty().WithMessage("Digite o Cpf")
            .MaximumLength(11).WithMessage("O campo Cpf não pode passar de 11 caracteres")
            .Matches("[0-9]{11}").WithMessage("Cpf inválido com pontos,traços ou letras")
            .MustAsync(CpfIsValid).WithMessage("CPF não encontrado no banco de dados ou está inativado");

        RuleForEach(o => o.Items)
           .ChildRules(item =>
           {
               item.RuleFor(i => i.ProductId)
                   .NotEmpty().WithMessage("O ID do produto não pode estar vazio")
                   .MustAsync(ProductIsValid).WithMessage("Id não encontrado no banco de dados ou o produto está inativado");

               item.RuleFor(i => i.ProductName)
                   .NotEmpty().WithMessage("Digite o nome do produto")
                   .MustAsync(ProductNameIsValid).WithMessage("Não existe um produto com este nome");

               item.RuleFor(i => i.Quantity)
                   .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero");
           });
        _productReadRepository = productReadRepository;
    }

    private async Task<bool> CpfIsValid(string cpf, CancellationToken cancellationToken)
    {
        var customer = await _customerReadRepository.GetByCpf(cpf, cancellationToken);
        
        return customer is not null && customer.Status is not Domain.Enums.EntityStatus.Inactive;
    }

    private async Task<bool> ProductIsValid(Guid id, CancellationToken cancellationToken)
    {
        var productId = await _productReadRepository.GetById(id, cancellationToken);

        return productId is not null && productId.Status is not EntityStatus.Inactive;
    }

    private async Task<bool> ProductNameIsValid(string name, CancellationToken cancellationToken)
    {
        var productName = await _productReadRepository.GetByName(name, cancellationToken);

        return productName is not null;
    }
}