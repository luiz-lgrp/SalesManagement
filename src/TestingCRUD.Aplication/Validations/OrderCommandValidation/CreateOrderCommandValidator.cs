using FluentValidation;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Validations.OrderCommandValidation;
public class CreateOrderCommandValidator : AbstractValidator<OrderInputModel>
{
    private ICustomerReadRepository _customerReadRepository;
    public CreateOrderCommandValidator(ICustomerReadRepository customerReadRepository)
    {
        _customerReadRepository = customerReadRepository;

        RuleFor(o => o.Cpf)
            .NotEmpty().WithMessage("Digite o Cpf")
            .MaximumLength(11).WithMessage("O campo Cpf não pode passar de 11 caracteres")
            .Matches("[0-9]{11}").WithMessage("Cpf inválido com pontos,traços ou letras")
            .MustAsync(CpfIsValid).WithMessage("CPF não encontrado no banco de dados ou está inativado");

        //RuleForEach(o => o.Items)
        //   .ChildRules(item =>
        //   {
        //       item.RuleFor(i => i.ProductId)
        //           .NotEmpty().WithMessage("O ID do produto não pode estar vazio");

        //       item.RuleFor(i => i.ProductName)
        //           .NotEmpty().WithMessage("Digite o nome do produto");

        //       item.RuleFor(i => i.Quantity)
        //           .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero");

        //   });

    }
    //TODO: Validar nome e id do produto
    private async Task<bool> CpfIsValid(string cpf, CancellationToken cancellationToken)
    {
        var customer = await _customerReadRepository.GetByCpf(cpf, cancellationToken);
        
        return customer is not null && customer.Status is not Domain.Enums.EntityStatus.Inactive;
    }
}
