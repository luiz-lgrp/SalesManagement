using MediatR;

namespace TestingCRUD.Application.Commands.CustomerCommands
{
    public class InactivateCustomerCommand : IRequest<bool>
    {
        public string Cpf { get; set; }

        public InactivateCustomerCommand(string cpf)
        {
            Cpf = cpf;
        }
    }
}
