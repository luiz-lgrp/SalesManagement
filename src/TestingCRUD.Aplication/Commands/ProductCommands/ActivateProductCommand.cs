using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class ActivateProductCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }

    public ActivateProductCommand(Guid id) => ProductId = id;
}
