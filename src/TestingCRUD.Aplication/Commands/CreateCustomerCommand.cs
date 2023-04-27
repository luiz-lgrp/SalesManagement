using TestingCRUD.Domain.Models;
using TestingCRUD.Aplication.InputModels;

using MediatR;

namespace TestingCRUD.Aplication.Commands
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
