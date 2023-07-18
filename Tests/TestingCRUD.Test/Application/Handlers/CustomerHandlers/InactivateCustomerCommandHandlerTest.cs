namespace TestingCRUD.Test.Application.Handlers.CustomerHandlers;

public class InactivateCustomerCommandHandlerTest
{
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private Mock<ICustomerReadRepository> _customerReadRepositoryMock;

    public InactivateCustomerCommandHandlerTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerReadRepositoryMock = new Mock<ICustomerReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsTrueAndCustomerStatusIsInactive()
    {
        // Arrange
        var customer = new Customer("TestName", "14687955866", "teste@example.com", "21-98568-8547");

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var requestComand = new InactivateCustomerCommand(customer.Cpf);

        var handler = new InactivateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(requestComand, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        customer.Status.ShouldBe(EntityStatus.Inactive);
    }

    [Fact]
    public async Task Handle_CustomerNotFound_ReturnsFalse()
    {
        // Arrange
        _customerReadRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync((Customer)null);

        var requestComand = new InactivateCustomerCommand("14526547899");

        var handler = new InactivateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(requestComand, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveChangeAsyncIsCalledOnce()
    {
        // Arrange
        var customer = new Customer("TestName", "14687955866", "teste@example.com", "21-98568-8547");

        _customerReadRepositoryMock.Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var requestCommand = new InactivateCustomerCommand(customer.Cpf);

        var handler = new InactivateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object);

        // Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        // Assert
        _customerRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
