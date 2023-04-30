using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingCRUD.Domain.Models
{
    public class Order
    {
        private static readonly Random random = new Random();

        public Guid OrderId { get; private set; }
        public string OrderNumber { get; private set; }
        public DateTime RequestDate { get; private set; }
        public Customer customer { get; set; }
        public List<OrderItem> Items { get; set; }

        public Order()
        {
            OrderId = Guid.NewGuid();
            OrderNumber = GenerateOrderNumber();
            RequestDate = DateTime.Now;
        }

        private string GenerateOrderNumber()
        {
            char letter = (char)random.Next('A', 'Z' + 1);
            int number = random.Next(10, 100);
            return $"{letter}{number}";
        }
    }
}
