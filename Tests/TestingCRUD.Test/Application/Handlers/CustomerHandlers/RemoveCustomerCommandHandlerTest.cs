namespace TestingCRUD.Test.Application.Handlers.CustomerHandlers;

public class RemoveCustomerCommandHandlerTest
{
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private Mock<ICustomerReadRepository> _customerReadRepositoryMock;

    public RemoveCustomerCommandHandlerTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerReadRepositoryMock = new Mock<ICustomerReadRepository>();
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ReturnsFalse()
    {
        // Arrange
        _customerReadRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        var requestCommand = new RemoveCustomerCommand("14526547899");

        var handler = new RemoveCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_ValidCommand_DeleteAsyncCalledOnceAndReturnsTrue()
    {
        // Arrange
        var customer = new Customer("TestName", "14687955866", "teste@example.com", "21-98568-8547");

        _customerReadRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _customerRepositoryMock.Setup(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var requestCommand = new RemoveCustomerCommand(customer.Cpf);

        var handler = new RemoveCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        _customerRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

}
