using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Shouldly;
using Xunit;
using TestingCRUD.Application.Commands.OrderItemCommands;
using TestingCRUD.Application.Handlers.OrderItemHandlers;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels;
using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Test.Application.Handlers.OrderItemHandlers
{
    public class AddItemOnOrderCommandHandlerTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IOrderReadRepository> _orderReadRepositoryMock;
        private readonly Mock<IValidator<OrderItemInputModel>> _validatorMock;
        private readonly Mock<IProductReadRepository> _productReadRepositoryMock;
        private readonly Mock<IProductRepository> _productRepositoryMock;

        public AddItemOnOrderCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderReadRepositoryMock = new Mock<IOrderReadRepository>();
            _validatorMock = new Mock<IValidator<OrderItemInputModel>>();
            _productReadRepositoryMock = new Mock<IProductReadRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task Handle_OrderFound_ItemAdded_ReturnsOrderViewModel()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newItemModel = new OrderItemInputModel
            {
                ProductId = productId,
                ProductName = "Product 1",
                Quantity = 2
            };
            var order = new Order("14625698755");
            var product = new Product("Product 1", 10, 10.0m) { Id = productId };

            _validatorMock
                .Setup(v => v.ValidateAsync(newItemModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _productReadRepositoryMock
                .Setup(x => x.GetById(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);

            _orderRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            _productRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new AddItemOnOrderCommandHandler(
                _orderRepositoryMock.Object,
                _orderReadRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object,
                _productRepositoryMock.Object);

            var command = new AddItemOnOrderCommand(orderId, newItemModel);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.OrderId.ShouldBe(order.Id);
            result.Items.Count.ShouldBe(1);
            result.Items[0].ProductName.ShouldBe("Product 1");
            result.Items[0].Quantity.ShouldBe(2);
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsNull()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newItemModel = new OrderItemInputModel
            {
                ProductId = productId,
                ProductName = "Product 1",
                Quantity = 2
            };

            _validatorMock
                .Setup(v => v.ValidateAsync(newItemModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var handler = new AddItemOnOrderCommandHandler(
                _orderRepositoryMock.Object,
                _orderReadRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object,
                _productRepositoryMock.Object);

            var command = new AddItemOnOrderCommand(orderId, newItemModel);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
            _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ProductNotFound_ReturnsNull()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newItemModel = new OrderItemInputModel
            {
                ProductId = productId,
                ProductName = "Product 1",
                Quantity = 2
            };
            var order = new Order("14625698755");

            _validatorMock
                .Setup(v => v.ValidateAsync(newItemModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _productReadRepositoryMock
                .Setup(x => x.GetById(productId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var handler = new AddItemOnOrderCommandHandler(
                _orderRepositoryMock.Object,
                _orderReadRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object,
                _productRepositoryMock.Object);

            var command = new AddItemOnOrderCommand(orderId, newItemModel);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeNull();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
            _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidationFails_ThrowsValidationException()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newItemModel = new OrderItemInputModel
            {
                ProductId = productId,
                ProductName = "Product 1",
                Quantity = 2
            };

            var validationFailures = new[]
            {
                new ValidationFailure("ProductId", "Não existe um produto com este Id")
            };
            var validationResult = new ValidationResult(validationFailures);

            _validatorMock
                .Setup(v => v.ValidateAsync(newItemModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            var handler = new AddItemOnOrderCommandHandler(
                _orderRepositoryMock.Object,
                _orderReadRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object,
                _productRepositoryMock.Object);

            var command = new AddItemOnOrderCommand(orderId, newItemModel);

            // Act & Assert
            await Should.ThrowAsync<ValidationException>(() => handler.Handle(command, CancellationToken.None));
        }
    }
}
