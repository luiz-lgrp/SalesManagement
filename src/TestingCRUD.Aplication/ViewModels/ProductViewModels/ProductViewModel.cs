using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Application.ViewModels.ProductViewModels;
public class ProductViewModel
{
    public string ProductName { get; }
    public int Stock { get; }
    public decimal Price { get; }
    public EntityStatus Status { get; }
}
