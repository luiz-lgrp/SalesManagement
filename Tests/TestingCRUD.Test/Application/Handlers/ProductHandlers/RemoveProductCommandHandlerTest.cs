namespace TestingCRUD.Test.Application.Handlers.ProductHandlers;

public class RemoveProductCommandHandlerTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IProductReadRepository> _productReadRepositoryMock;

    public RemoveProductCommandHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productReadRepositoryMock = new Mock<IProductReadRepository>();
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsFalse()
    {
        //Arrange
        _productReadRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var requestCommand = new RemoveProductCommand(Guid.Parse("6bc099b6-e78d-4fa6-9e3f-58ed7c7a32e7"));

        var handler = new RemoveProductCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        //Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_ValidCommand_DeleteAsynCalledOnceAndReturnsTrue()
    {
        //Arrange
        var product = new Product("Suporte p/ celular", 10, 100.5m);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _productRepositoryMock
            .Setup(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var requestCommand = new RemoveProductCommand(product.Id);

        var handler = new RemoveProductCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        //Assert
        result.ShouldBeTrue();
        _productRepositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
