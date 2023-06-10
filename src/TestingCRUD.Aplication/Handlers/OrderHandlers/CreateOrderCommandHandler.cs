using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.OrderCommands;
using TestingCRUD.Application.Validations.OrderCommandValidation;
using TestingCRUD.Application.Dtos;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels;

namespace TestingCRUD.Application.Handlers.OrderHandlers;
public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator<OrderInputModel> _validator;
    private readonly IProductReadRepository _productReadRepository;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository, 
        IValidator<OrderInputModel> validator, 
        IProductReadRepository productReadRepository)
    {
        _orderRepository = orderRepository;
        _validator = validator;
        _productReadRepository = productReadRepository;
    }

    public async Task<OrderViewModel> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var cpfCustomer = request.Order.Cpf;
        var pedidoItems = request.Order.Items;

        var validationResult = await _validator.ValidateAsync(request.Order);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var order = new Order(cpfCustomer);

        var productIds = pedidoItems.Select(item => item.ProductId).ToList();
        var products = await _productReadRepository.GetProductsByIds(productIds, cancellationToken);

        foreach (var itemDto in pedidoItems)
        {
            var product = products.FirstOrDefault(p => p.Id == itemDto.ProductId);

            product!.DecrementStock(itemDto.Quantity);

            OrderItem item = new OrderItem(itemDto.ProductId, itemDto.ProductName, itemDto.Quantity, product.Price);
            
            order.AddItemToOrder(item);
        }

        var createdOrder = await _orderRepository.CreateAsync(order, cancellationToken);

        if (createdOrder is null)
            return null;

        var orderDto = new OrderViewModel
        {
            OrderId = createdOrder.Id,
            Cpf = createdOrder.Cpf,
            OrderCode = createdOrder.OrderCode,
            TotalValue = createdOrder.TotalValue,
            Status = createdOrder.Status,
            Items = createdOrder.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitValue = item.UnitValue
            }).ToList(),
        };

        return orderDto;
    }
}
