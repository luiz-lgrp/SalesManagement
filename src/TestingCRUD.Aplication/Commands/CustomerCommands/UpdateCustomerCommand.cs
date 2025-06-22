using MediatR;
using TestingCRUD.Aplication.Shared;
using TestingCRUD.Application.InputModels;

namespace TestingCRUD.Application.Commands.CustomerCommands;
public class UpdateCustomerCommand : IRequest<Result<bool>>
{
    public string Cpf { get; set; }
    public CustomerInputModel UpdateCustomer { get; set; }

    public UpdateCustomerCommand(string cpf, CustomerInputModel updateCustomer)
    {
        Cpf = cpf;
        UpdateCustomer = updateCustomer;
    }
}
