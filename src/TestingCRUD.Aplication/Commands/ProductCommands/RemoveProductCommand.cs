using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class RemoveProductCommand : IRequest<bool>
{
    public Guid Id { get; set; }

    public RemoveProductCommand(Guid id) => Id = id;
}
