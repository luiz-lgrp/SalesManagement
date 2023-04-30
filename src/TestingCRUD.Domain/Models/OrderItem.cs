namespace TestingCRUD.Domain.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalValue  { get; private set; }
        public Product Product { get; set; }

        public OrderItem()
        {
            OrderItemId = Guid.NewGuid();
            TotalValue = Quantity * Product!.Value;
        }
    }
}
