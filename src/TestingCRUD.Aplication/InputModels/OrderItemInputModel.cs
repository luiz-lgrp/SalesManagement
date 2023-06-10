namespace TestingCRUD.Application.InputModels;
public class OrderItemInputModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
