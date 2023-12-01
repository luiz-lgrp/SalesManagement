using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models;
public class Product : BaseModel
{
    public string ProductName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public EntityStatus Status { get; private set; }

    public Product(string productName, int stock, decimal price)
    {
        ProductName = productName;
        Stock = stock;
        Price = price;
        Status = EntityStatus.Active;
    }

    public void Inactive()
    {
        if (Status == EntityStatus.Inactive)
            throw new ArgumentException("O Status já está Inativo");

        Status = Enums.EntityStatus.Inactive;
        Updated = DateTime.Now;
    }

    public void Active()
    {
        if (Status == EntityStatus.Active)
            throw new ArgumentException("O Status já está ativo");

        Status = Enums.EntityStatus.Active;
        Updated = DateTime.Now;
    }

    public bool HaveStock(int quantity) => Stock >= quantity;

    public void DecrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("A quantidade a ser decrementada em estoque deve ser maior que zero");
        if (!HaveStock(quantity))
            throw new ArgumentException("Quantidade em estoque insuficiente");

        Stock -= quantity;
        Updated = DateTime.Now;
    }

    public void IncreaseStock(int quantity) 
    {
        if (quantity <= 0) 
            throw new ArgumentException("A quantidade a ser Incrementada em estoque deve ser maior que zero");

        Stock += quantity;
        Updated = DateTime.Now;
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0) 
            throw new ArgumentException("O preço deve ser maior que zero");
        if (price == Price)
            throw new ArgumentException($"O valor do produto já é {price}");

        Price = price;
        Updated = DateTime.Now;
    }
    
}
