namespace TestingCRUD.Test.Application.Handlers.OrderItemHandlers
{
    public class UpdateQuantityItemCommandHandlerTest
    {
        private readonly Mock<IOrderRepository> _orderRepositoryMock;
        private readonly Mock<IOrderReadRepository> _orderReadRepositoryMock;

        public UpdateQuantityItemCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderReadRepositoryMock = new Mock<IOrderReadRepository>();
        }

        [Fact]
        public async Task Handle_OrderFound_ItemQuantityUpdated_ReturnsTrue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newQuantity = 5;
            var order = new Order("14625698755");
            var orderItem = new OrderItem(productId, "Fone de celular", 2, 10.0m);
            order.AddItemToOrder(orderItem);

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var handler = new UpdateQuantityItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new UpdateQuantityItemCommand(orderId, new ChangeQuantityItemDTO(productId, newQuantity));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeTrue();
            orderItem.Quantity.ShouldBe(newQuantity);
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var newQuantity = 5;

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var handler = new UpdateQuantityItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new UpdateQuantityItemCommand(orderId, new ChangeQuantityItemDTO(productId, newQuantity));

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
            var newQuantity = 5;
            var order = new Order("14625698755");

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = new UpdateQuantityItemCommandHandler(_orderRepositoryMock.Object, _orderReadRepositoryMock.Object);

            var command = new UpdateQuantityItemCommand(orderId, new ChangeQuantityItemDTO(productId, newQuantity));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            _orderRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }
    }
}
