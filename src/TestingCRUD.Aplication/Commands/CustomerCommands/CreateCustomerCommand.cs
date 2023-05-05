using MediatR;

using TestingCRUD.Domain.Models;
using TestingCRUD.Application.CustomerInputModels;

namespace TestingCRUD.Application.Commands.CustomerCommands
{//TODO: Update não retorna nada (so um bool) ou create se for pra retornar retorno um view model
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public CreateInputModel CreateCustomer { get; set; }

        public CreateCustomerCommand(CreateInputModel createCustomer)
        {
            CreateCustomer = createCustomer;
        }
    }
}
