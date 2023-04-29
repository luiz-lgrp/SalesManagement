using MediatR;

using TestingCRUD.Domain.Models;
using TestingCRUD.Aplication.CustomerInputModels;

namespace TestingCRUD.Aplication.Commands.CustomerCommands
{
    public class UpdateCustomerCommand : IRequest<Customer>
    {
        public string Cpf { get; set; }
        public UpdateInputModel UpdateCustomer { get; set; }

        public UpdateCustomerCommand(string cpf, UpdateInputModel updateCustomer)
        {
            Cpf = cpf;
            UpdateCustomer = updateCustomer;
        }
    }
}
