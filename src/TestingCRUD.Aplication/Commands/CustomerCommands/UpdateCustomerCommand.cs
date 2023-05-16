using MediatR;

using TestingCRUD.Application.InputModels.CustomerInputModels;

namespace TestingCRUD.Application.Commands.CustomerCommands;
public class UpdateCustomerCommand : IRequest<bool>
{
    public string Cpf { get; set; }
    public CustomerInputModel UpdateCustomer { get; set; }

    public UpdateCustomerCommand(string cpf, CustomerInputModel updateCustomer)
    {
        Cpf = cpf;
        UpdateCustomer = updateCustomer;
    }
}
