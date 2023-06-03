using MediatR;

namespace TestingCRUD.Application.Commands.ProductCommands;
public class ChangePriceCommand : IRequest<bool>
{
    public Guid ProductId { get; set; }
    public decimal Price { get; set; }

    public ChangePriceCommand(Guid productId, decimal price)
    {
        ProductId = productId;
        Price = price;
    }
}
