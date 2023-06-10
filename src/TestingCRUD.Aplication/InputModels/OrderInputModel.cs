namespace TestingCRUD.Application.InputModels;
public class OrderInputModel
{
    public string Cpf { get; set; }
    public List<OrderItemInputModel> Items { get; set; }
}
