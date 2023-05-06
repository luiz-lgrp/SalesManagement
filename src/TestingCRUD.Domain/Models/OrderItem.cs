namespace TestingCRUD.Domain.Models
{
    public class OrderItem : BaseModel
    {
        public Guid ProductId { get; private set; }
        public string? OrderCode { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue  { get; private set; }
        public Order? Order { get; set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }
        
        public void LinkOrder(string orderCode) => OrderCode = orderCode;

        public decimal CalculateValue() => UnitValue * Quantity;

        public void AddQuantity(int  quantity) => Quantity += quantity;

        public void UpdateQuantity(int quantity) => Quantity = quantity;
    }
}
