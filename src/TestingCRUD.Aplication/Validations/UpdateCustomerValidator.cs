using TestingCRUD.Domain.InputModels;

using FluentValidation;

namespace TestingCRUD.Aplication.Validations
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateInputModel>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(c => c.Name)
               .NotEmpty().WithMessage("Por favor, digite o seu nome.")
               .MaximumLength(30).WithMessage("O campo nome não pode passar de 30 caracteres.")
               .MinimumLength(3).WithMessage("O campo nome não pode ser menor que 02 caracteres.");


            RuleFor(c => c.Cpf)
                .NotEmpty().WithMessage("Por favor, digite o seu Cpf.")
                .MaximumLength(11).WithMessage("O campo Cpf não pode passar de 11 caracteres.")
                .Matches("[0-9]{11}").WithMessage("Cpf inválido com pontos,traços ou letras.");

            RuleFor(c => c.Birthday)
                .NotEmpty().WithMessage("Por favor, digite a sua data de nascimento.")
                .Matches(@"^(0?[1-9]|[12][0-9]|3[01])\/(0?[1-9]|1[0-2])\/\d{4}$").WithMessage("O formato da data deve ser DD/MM/AAAA.");

            RuleFor(c => c.Phone)
                .NotEmpty().WithMessage("Por favor digite um telefone")
                .Matches("^([1-9]{2})-(?:[2-8]|9[1-9])[0-9]{3}-[0-9]{4}$").WithMessage("Formato de telefone inválido xx-xxxxx-xxxx")
                .MaximumLength(14).WithMessage("telefone não pode ter mais de 11 dígitos contando com DDD");

            RuleFor(c => c.Status)
                .IsInEnum().WithMessage("Insira um status válido, 1 para Ativo e 0 para Inativo");
        }
    }
}
