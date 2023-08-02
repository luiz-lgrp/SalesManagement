using FluentValidation;

using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Validations.CustomerCommandValidation;
public class CreateCustomerValidator : UpdateCustomerValidator
{
    private readonly ICustomerReadRepository _customerReadRepository;

    public CreateCustomerValidator(ICustomerReadRepository customerReadRepository)
    {
        _customerReadRepository = customerReadRepository;

        RuleFor(c => c.Cpf)
            .MustAsync(CpfIsValid).WithMessage("Este Cpf já está cadastrado");
    }

    private async Task<bool> CpfIsValid(string cpf, CancellationToken cancellationToken)
    {
        var customer = await _customerReadRepository.GetByCpf(cpf, cancellationToken);

        return customer is null;
    }
}