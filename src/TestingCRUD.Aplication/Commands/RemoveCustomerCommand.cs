using MediatR;

namespace TestingCRUD.Aplication.Commands
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
