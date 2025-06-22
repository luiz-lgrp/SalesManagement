using MediatR;
using TestingCRUD.Aplication.InputModels;
using TestingCRUD.Aplication.Shared;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels.CustomerViewModels;

namespace TestingCRUD.Application.Commands.CustomerCommands;
public class CreateCustomerCommand : IRequest<Result<CustomerViewModel>>
{
    public CreateCustomerInputModel CreateCustomer { get; set; }

    public CreateCustomerCommand(CreateCustomerInputModel createCustomer) 
        => CreateCustomer = createCustomer;
}
