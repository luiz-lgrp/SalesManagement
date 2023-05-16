using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models;
public class Product : BaseModel
{
    public string ProductName { get; private set; }
    public int Stock { get; private set; }
    public decimal Price { get; private set; }
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
        Status = Enums.EntityStatus.Inactive;
        Updated = DateTime.Now;
    }

    public void Active()
    {
        Status = Enums.EntityStatus.Active;
        Updated = DateTime.Now;
    }

    public bool HaveStock(int quantity) => Stock >= quantity;

    public void DecrementStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantidade inválida");
        if (!HaveStock(quantity))
            throw new ArgumentException("Quantidade em estoque insuficiente");

        Stock -= quantity;
        Updated = DateTime.Now;
    }

    public void IncreaseStock(int quantity) 
    {
        if (quantity <= 0) 
            throw new ArgumentException("Quantidade inválida");

        Stock += quantity;
        Updated = DateTime.Now;
    }

    public void ChangePrice(decimal price)
    {
        if (Price <= 0) 
            throw new ArgumentException("O preço deve ser maior que zero");

        Price = price;
        Updated = DateTime.Now;
    }
    
}
