using MediatR;

using TestingCRUD.Domain.Models;
using TestingCRUD.Aplication.CustomerInputModels;

namespace TestingCRUD.Aplication.Commands.CustomerCommands
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public CreateInputModel CreateCustomer { get; set; }

        public CreateCustomerCommand(CreateInputModel createCustomer)
        {
            CreateCustomer = createCustomer;
        }
    }
}
