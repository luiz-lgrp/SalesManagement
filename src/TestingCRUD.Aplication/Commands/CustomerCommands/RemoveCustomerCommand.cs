using MediatR;

namespace TestingCRUD.Application.Commands.CustomerCommands
{
    public class RemoveCustomerCommand : IRequest
    {
        public string Cpf { get; set; }

        public RemoveCustomerCommand(string cpf)
        {
            Cpf = cpf;
        }
    }
}
