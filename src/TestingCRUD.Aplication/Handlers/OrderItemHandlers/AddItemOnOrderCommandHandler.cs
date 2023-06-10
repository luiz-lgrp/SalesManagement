using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Application.Dtos;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Commands.OrderItemCommands;

namespace TestingCRUD.Application.Handlers.OrderItemHandlers;

public class AddItemOnOrderCommandHandler : IRequestHandler<AddItemOnOrderCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IValidator<OrderItemInputModel> _validator;
    private readonly IProductReadRepository _productReadRepository;

    public AddItemOnOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderReadRepository orderReadRepository,
        IValidator<OrderItemInputModel> validator,
        IProductReadRepository productReadRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
        _validator = validator;
        _productReadRepository = productReadRepository;
    }
    //TODO: Adicionar item deu ruim, verificar
    public async Task<OrderViewModel?> Handle(AddItemOnOrderCommand request, CancellationToken cancellationToken)
    {
        var orderId = request.OrderId;
        var newItemModel = request.NewItem;

        var validationResult = await _validator.ValidateAsync(newItemModel);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var order = await _orderReadRepository.GetById(orderId, cancellationToken);

        if (order is null)
            return null;

        var product = await _productReadRepository.GetById(newItemModel.ProductId, cancellationToken);

        if (product is null)
            return null;

        if (!product.HaveStock(newItemModel.Quantity))
            throw new ArgumentException("Quantidade em estoque insuficiente");


        var newItem = new OrderItem(
            newItemModel.ProductId, 
            newItemModel.ProductName, 
            newItemModel.Quantity, 
            product.Price);

        order.AddItemToOrder(newItem);

        await _orderRepository.SaveChangesAsync();

        var orderVM = new OrderViewModel
        {
            OrderId = order.Id,
            Cpf = order.Cpf,
            Items = order.OrderItems.Select(item => new OrderItemDto
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                UnitValue = item.UnitValue
            }).ToList(),
        };

        return orderVM;
    }
}
