using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class InactivateProductCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }

    public InactivateProductCommand(Guid id) => ProductId = id;
}
