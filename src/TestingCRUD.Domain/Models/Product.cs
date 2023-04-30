namespace TestingCRUD.Domain.Models
{
    public class Product : BaseModel
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Stock { get; set; }
        public decimal Value { get; set; }

        public Product(string productName, int stock, decimal value)
        {
            ProductId = Guid.NewGuid();
            ProductName = productName;
            Stock = stock;
            Value = value;
        }
    }
}
