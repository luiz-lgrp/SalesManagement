using MediatR;

namespace TestingCRUD.Application.Commands.CustomerCommands;
public class ActivateCustomerCommand : IRequest<bool>
{
    public string Cpf { get; set; }

    public ActivateCustomerCommand(string cpf) => Cpf = cpf;  
}
