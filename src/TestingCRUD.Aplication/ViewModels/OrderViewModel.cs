using TestingCRUD.Domain.Enums;
using TestingCRUD.Application.Dtos;

namespace TestingCRUD.Application.ViewModels;
public class OrderViewModel
{
    public Guid OrderId { get; set; }
    public string Cpf { get; set; }
    public string OrderCode { get; set; }
    public decimal TotalValue { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemDto> Items { get; set; }
}
