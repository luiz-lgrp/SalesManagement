using FluentValidation;

using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Validations.CustomerCommandValidation;
public class UpdateCustomerValidator : AbstractValidator<CustomerInputModel>
{
    public UpdateCustomerValidator() 
    {

        RuleFor(c => c.Name)
           .NotEmpty().WithMessage("Digite um nome")
           .MaximumLength(30).WithMessage("O campo nome não pode passar de 30 caracteres")
           .MinimumLength(3).WithMessage("O campo nome não pode ser menor que 02 caracteres");


        RuleFor(c => c.Cpf)
            .NotEmpty().WithMessage("Digite o seu Cpf")
            .MaximumLength(11).WithMessage("O campo Cpf não pode passar de 11 caracteres")
            .Matches("[0-9]{11}").WithMessage("Cpf inválido com pontos,traços ou letras");
            

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Digite um email")
            .EmailAddress().WithMessage("Formato de email inválido");

        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Digite um telefone")
            .Matches("^([1-9]{2})-(?:[2-8]|9[1-9])[0-9]{3}-[0-9]{4}$").WithMessage("Formato de telefone inválido xx-xxxxx-xxxx")
            .MaximumLength(14).WithMessage("Telefone não pode ter mais de 11 dígitos contando com DDD");
    }
}