using TestingCRUD.Application.Dtos;

namespace TestingCRUD.Application.ViewModels;
public class OrderViewModel
{
    public Guid OrderId { get; set; }
    public string Cpf { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
