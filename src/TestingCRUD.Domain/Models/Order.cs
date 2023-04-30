using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models
{
    public class Order : BaseModel
    {
        private List<OrderItem> _orderItems;
        private static readonly Random random = new Random();

        public Guid CustomerId { get; private set; }
        public string OrderCode { get; private set; }
        public decimal TotalValue { get; private set; }
        public OrderStatus  Status { get; private set; }

        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            OrderCode = GenerateOrderCode();
            Status = OrderStatus.Novo;
            _orderItems = new List<OrderItem>();
        }

        private string GenerateOrderCode()
        {
            char letter = (char)random.Next('A', 'Z' + 1);
            int number = random.Next(10, 100);
            return $"{letter}{number}";
        }

        public void CalculateTotalAmount() => TotalValue = _orderItems.Sum(orderItem => orderItem.CalculateValue());


    }
}
