using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models;
public class Order : BaseModel
{
    private List<OrderItem> _orderItems;
    private static readonly Random random = new Random();
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

    public string Cpf { get; private set; }
    public string OrderCode { get; private set; }
    public decimal TotalValue { get; private set; }
    public OrderStatus  Status { get; private set; }

    public Order(string cpf)
    {
        Cpf = cpf;
        OrderCode = GenerateOrderCode();
        Status = OrderStatus.New;
        _orderItems = new List<OrderItem>();
    }

    private string GenerateOrderCode()
    {
        char letter = (char)random.Next('A', 'Z' + 1);
        int number = random.Next(10, 100);
        return $"{letter}{number}";
    }

    public void CalculateTotalAmount() => TotalValue = _orderItems.Sum(orderItem => orderItem.CalculateValue());

    public void WaitForPayment()
    {
        Status = OrderStatus.AwaitingPayment;
        Updated = DateTime.Now;
    }

    public void Conclude()
    {
        Status = OrderStatus.Concluded;
        Updated = DateTime.Now;
    }

    private OrderItem? ExistOrderItem(OrderItem item) =>
        _orderItems.FirstOrDefault(orderItem => orderItem.ProductId == item.ProductId);

    public void AddItemToOrder(OrderItem item)
    {
        item.LinkOrder(OrderCode);

        if (ExistOrderItem(item) is var itemFound && itemFound is not null)
        {
            itemFound.AddQuantity(itemFound.Quantity);
            item = itemFound;
        }
        else
        {
            _orderItems.Add(item);
        }

        item.CalculateValue();
        CalculateTotalAmount();
        Updated = DateTime.Now;
    }
    //Fazer a actions de Atualizar quantidade do item no pedido
    public void UpdateQuantityItem(OrderItem item, int newQuantity)
    {
        if (ExistOrderItem(item) is var itemFound && itemFound is null)
            throw new Exception("Item não encontrado, item inválido");

        itemFound.UpdateQuantity(newQuantity);
        CalculateTotalAmount();
        Updated = DateTime.Now;
    }
    //Fazer a actions de remover item do pedido
    public void RemoveItem(OrderItem item)
    {
        if (ExistOrderItem(item) is var itemFound && itemFound is null)
            throw new Exception("Item não encontrado, item inválido");

        _orderItems.Remove(itemFound);
        CalculateTotalAmount();
        Updated = DateTime.Now;
    }
}
