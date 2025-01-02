using Microsoft.EntityFrameworkCore;
using TestingCRUD.Domain.Models;

namespace TestingCRUD.Infra;

public static class InitializeDB
{
    public static void Initialize(Context context)
    {
        context.Database.Migrate();

        if (context.Customers.Any() && context.Products.Any()
            && context.Orders.Any() && context.OrderItems.Any())
        {
            return;
        }

        var customers = new Customer[]
        {
            new Customer("João Silva", "12345678901", "joao@email.com", "11999990000"),
            new Customer("Maria Santos", "98765432100", "maria@email.com", "21988887777"),
            new Customer("Carlos Oliveira", "11122233344", "carlos@email.com", "31977776666")
        };

        foreach (var customer in customers)
        {
            context.Customers.Add(customer);
        }

        var products = new Product[]
        {
            new Product("Teclado Mecânico", 10, 250.50m),
            new Product("Mouse Gamer", 15, 150.75m),
            new Product("Monitor 27", 8, 1200.00m),
            new Product("Notebook Dell", 5, 5500.00m),
            new Product("Cadeira Ergonômica", 12, 980.00m),
            new Product("HD Externo 1TB", 20, 350.00m)
        };

        foreach (var product in products)
        {
            context.Products.Add(product);
        }

        var orders = new Order[]
        {
            new Order(customers[0].Cpf),
            new Order(customers[1].Cpf),
            new Order(customers[2].Cpf)
        };

        foreach (var order in orders)
        {
            context.Orders.Add(order);
        }

        var orderItems = new OrderItem[]
        {
            new OrderItem(products[0].Id, products[0].ProductName, 2, products[0].Price),
            new OrderItem(products[1].Id, products[1].ProductName, 3, products[1].Price),
            new OrderItem(products[2].Id, products[2].ProductName, 1, products[2].Price),

            new OrderItem(products[3].Id, products[3].ProductName, 1, products[3].Price),
            new OrderItem(products[4].Id, products[4].ProductName, 2, products[4].Price),
            new OrderItem(products[5].Id, products[5].ProductName, 4, products[5].Price),

            new OrderItem(products[0].Id, products[0].ProductName, 1, products[0].Price),
            new OrderItem(products[1].Id, products[1].ProductName, 2, products[1].Price),
            new OrderItem(products[2].Id, products[2].ProductName, 2, products[2].Price)
        };

        // Relacionar Itens ao Pedido
        orders[0].AddItemToOrder(orderItems[0]);
        orders[0].AddItemToOrder(orderItems[1]);
        orders[0].AddItemToOrder(orderItems[2]);

        orders[1].AddItemToOrder(orderItems[3]);
        orders[1].AddItemToOrder(orderItems[4]);
        orders[1].AddItemToOrder(orderItems[5]);

        orders[2].AddItemToOrder(orderItems[6]);
        orders[2].AddItemToOrder(orderItems[7]);
        orders[2].AddItemToOrder(orderItems[8]);

        context.SaveChanges();
    }
}
