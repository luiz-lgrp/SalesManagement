using FluentValidation;
using TestingCRUD.Aplication.InputModels;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Validations.CustomerCommandValidation;
public class CreateCustomerValidator : AbstractValidator<CreateCustomerInputModel>
{
    private readonly ICustomerReadRepository _customerReadRepository;

    public CreateCustomerValidator(ICustomerReadRepository customerReadRepository)
    {
        _customerReadRepository = customerReadRepository;

        RuleFor(c => c.Cpf)
            .MustAsync(CpfIsValid).WithMessage("Este Cpf já está cadastrado");

        RuleFor(c => c.Cpf)
           .NotEmpty().WithMessage("Digite o seu Cpf")
           .MaximumLength(11).WithMessage("O campo Cpf não pode passar de 11 caracteres");

        RuleFor(c => c.Name)
           .NotEmpty().WithMessage("Digite um nome")
           .MaximumLength(30).WithMessage("O campo nome não pode passar de 30 caracteres")
           .MinimumLength(3).WithMessage("O campo nome não pode ser menor que 02 caracteres");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Digite um email")
            .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Digite um telefone")
            .MaximumLength(14).WithMessage("Telefone não pode ter mais de 11 dígitos contando com DDD");
    }

    private async Task<bool> CpfIsValid(string cpf, CancellationToken cancellationToken)
    {
        var customer = await _customerReadRepository.GetByCpf(cpf, cancellationToken);

        return customer is null;
    }
}