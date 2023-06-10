namespace TestingCRUD.Application.Dtos;
public class OrderItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitValue { get;  set; }
}
