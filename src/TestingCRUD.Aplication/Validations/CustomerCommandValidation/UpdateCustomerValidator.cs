using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Validations.CustomerCommandValidation;
public class UpdateCustomerValidator : CreateCustomerValidator
{
    public UpdateCustomerValidator(ICustomerReadRepository customerReadRepository) 
        : base(customerReadRepository)
    {
    }
}