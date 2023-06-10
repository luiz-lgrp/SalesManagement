using MediatR;

using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels.CustomerViewModels;

namespace TestingCRUD.Application.Commands.CustomerCommands;
public class CreateCustomerCommand : IRequest<CustomerViewModel>
{
    public CustomerInputModel CreateCustomer { get; set; }

    public CreateCustomerCommand(CustomerInputModel createCustomer) => CreateCustomer = createCustomer;
}
