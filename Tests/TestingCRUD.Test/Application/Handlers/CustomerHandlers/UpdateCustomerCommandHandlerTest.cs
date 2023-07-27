namespace TestingCRUD.Test.Application.Handlers.CustomerHandlers;

public class UpdateCustomerCommandHandlerTest
{
    
    private Mock<ICustomerRepository> _customerRepositoryMock;
    private Mock<ICustomerReadRepository> _customerReadRepositoryMock;
    public UpdateCustomerCommandHandlerTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerReadRepositoryMock = new Mock<ICustomerReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdateAsyncCalledOnceAndReturnsTrue()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Jhon",
            Cpf = "14785236912",
            Email = "jhon@example.com.br",
            Phone = "88-5522-8874"
        };

        var inputCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var customer = new Customer("Jhon", "14785236912", "jhon@example.com.br", "88-5522-8874");

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        var validator = new UpdateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act
        var result = await handler.Handle(inputCustomerCommand, CancellationToken.None);

        //Assert
        result.ShouldBeTrue();
        _customerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once());
    }


}
