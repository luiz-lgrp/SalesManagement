namespace TestingCRUD.Application.InputModels;
public class OrderInputModel
{
    public string Cpf { get; set; }
    public List<OrderItemInputModel> Items { get; set; }
}
public class OrderItemInputModel
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
}
