using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Application.ViewModels.ProductViewModels;
public class ProductViewModel
{//TODO: deveria ser strings?
    public Guid Id { get; set; }
    public string ProductName { get; }
    public int Stock { get; }
    public decimal Price { get; }
    public EntityStatus Status { get; }
}
