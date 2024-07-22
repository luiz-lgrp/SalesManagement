using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestingCRUD.Application.Commands.OrderCommands;
using TestingCRUD.Application.Handlers.OrderHandlers;

namespace TestingCRUD.Test.Application.Handlers.OrderHandlers
{
    public class RemoveOrderCommandHandlerTest
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IOrderReadRepository> _orderReadRepositoryMock;

        public RemoveOrderCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _orderReadRepositoryMock = new Mock<IOrderReadRepository>();
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Order)null);

            var handler = new RemoveOrderCommandHandler(_orderRepositoryMock.Object,_orderReadRepositoryMock.Object);

            var command = new RemoveOrderCommand(orderId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeFalse();
            _orderReadRepositoryMock.Verify(x => x.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_OrderFound_OrderRemoved_ReturnsTrue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = new Order("14625698755");

            _orderReadRepositoryMock
                .Setup(x => x.GetById(orderId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            _orderRepositoryMock
                .Setup(x => x.DeleteAsync(order.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var handler = new RemoveOrderCommandHandler(_orderRepositoryMock.Object,_orderReadRepositoryMock.Object);

            var command = new RemoveOrderCommand(orderId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.ShouldBeTrue();
            _orderReadRepositoryMock.Verify(x => x.GetById(orderId, It.IsAny<CancellationToken>()), Times.Once);
            _orderRepositoryMock.Verify(x => x.DeleteAsync(order.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
