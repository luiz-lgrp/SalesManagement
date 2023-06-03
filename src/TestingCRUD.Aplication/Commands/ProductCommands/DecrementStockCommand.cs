using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class DecrementStockCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }

    public DecrementStockCommand(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
