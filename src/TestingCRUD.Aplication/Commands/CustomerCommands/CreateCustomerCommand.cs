using MediatR;

using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Application.InputModels.CustomerInputModels;

namespace TestingCRUD.Application.Commands.CustomerCommands
{
    public class CreateCustomerCommand : IRequest<CustomerViewModel>
    {
        public CustomerInputModel CreateCustomer { get; set; }

        public CreateCustomerCommand(CustomerInputModel createCustomer)
        {
            CreateCustomer = createCustomer;
        }
    }
}
