namespace TestingCRUD.Domain.Models
{
    public class OrderItem : BaseModel
    {
        public Guid OrderId { get; private set; }//TODO: Tira isso?
        public Guid ProductId { get; private set; }
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

        public decimal CalculateValue()
        {
            return UnitValue * Quantity;
        }
    }
}
