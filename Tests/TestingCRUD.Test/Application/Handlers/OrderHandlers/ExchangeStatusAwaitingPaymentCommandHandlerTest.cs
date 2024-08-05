namespace TestingCRUD.Test.Application.Handlers.OrderHandlers
{
    public class ExchangeStatusAwaitingPaymentCommandHandlerTest
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IOrderReadRepository> _orderReadRepositoryMock;

        public ExchangeStatusAwaitingPaymentCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderReadRepositoryMock = new Mock<IOrderReadRepository>();
        }

        [Fact]
        public async Task Handle_OrderFound_StatusChangedToAwaitingPayment_ReturnsTrue()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = new Order("14612655599");

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new ExchangeStatusAwaitingPaymentCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new ExchangeStatusAwaitingPaymentCommand(orderId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeTrue();
            order.Status.ShouldBe(OrderStatus.AwaitingPayment);
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var handler = new ExchangeStatusAwaitingPaymentCommandHandler(
                _orderRepositoryMock.Object,
                _orderReadRepositoryMock.Object);

            var command = new ExchangeStatusAwaitingPaymentCommand(orderId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
