using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.InputModels;

using MediatR;

namespace TestingCRUD.Aplication.Commands
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
