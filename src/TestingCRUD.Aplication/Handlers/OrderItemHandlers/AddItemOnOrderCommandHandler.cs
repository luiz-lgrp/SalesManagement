using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Application.Dtos;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Commands.OrderItemCommands;
using Microsoft.EntityFrameworkCore;

namespace TestingCRUD.Application.Handlers.OrderItemHandlers;

public class AddItemOnOrderCommandHandler : IRequestHandler<AddItemOnOrderCommand, OrderViewModel>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderReadRepository _orderReadRepository;
    private readonly IValidator<OrderItemInputModel> _validator;
    private readonly IProductReadRepository _productReadRepository;
    private readonly IProductRepository _productRepository;

    public AddItemOnOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrderReadRepository orderReadRepository,
        IValidator<OrderItemInputModel> validator,
        IProductReadRepository productReadRepository,
        IProductRepository productRepository)
    {
        _orderRepository = orderRepository;
        _orderReadRepository = orderReadRepository;
        _validator = validator;
        _productReadRepository = productReadRepository;
        _productRepository = productRepository;
    }
    //TODO: Adicionar item deu ruim, verificar 
    public async Task<OrderViewModel?> Handle(AddItemOnOrderCommand request, CancellationToken cancellationToken)
    {
        try
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

            var newItem = new OrderItem(
                product.Id,
                product.ProductName,
                newItemModel.Quantity,
                product.Price);

            order.AddItemToOrder(newItem);
            await _orderRepository.SaveChangesAsync();

            //Depois de funcionar, mudar para o bloco acima
            product.DecrementStock(newItemModel.Quantity);
            await _productRepository.SaveChangesAsync();


            var orderVM = new OrderViewModel
            {
                OrderId = order.Id,
                Cpf = order.Cpf,
                OrderCode = order.OrderCode,
                TotalValue = order.TotalValue,
                Status = order.Status,
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
        catch (Exception e)
        {

        }

        return null;
    }
}
