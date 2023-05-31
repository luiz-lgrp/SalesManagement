using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Application.ViewModels.ProductViewModels;
public class ProductViewModel
{
    public Guid Id { get; set; }
    public string ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public EntityStatus Status { get; set; }
}
