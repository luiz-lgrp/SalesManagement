using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class IncreaseStockCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public IncreaseStockCommand(Guid id, int quantity)
    {
        ProductId = id;
        Quantity = quantity;
    }
}
