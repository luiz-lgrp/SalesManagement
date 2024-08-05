namespace TestingCRUD.Test.Application.Handlers.OrderItemHandlers
{
    public class RemoveItemCommandHandlerTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IOrderReadRepository> _orderReadRepositoryMock;

        public RemoveItemCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderReadRepositoryMock = new Mock<IOrderReadRepository>();
        }

        [Fact]
        public async Task Handle_OrderFound_ItemRemoved_ReturnsTrue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var order = new Order("14625698755");
            var orderItem = new OrderItem(productId, "Fone de ouvido", 2, 10.0m);
            order.AddItemToOrder(orderItem);

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new RemoveItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new RemoveItemCommand(orderId, productId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeTrue();
            order.OrderItems.ShouldNotContain(orderItem);
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var handler = new RemoveItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new RemoveItemCommand(orderId, productId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_OrderFound_ItemNotFound_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var order = new Order("14625698755");

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = new RemoveItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new RemoveItemCommand(orderId, productId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}

