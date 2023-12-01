namespace TestingCRUD.Test.Application.Handlers.ProductHandlers;

public class ChangePriceCommandHandlerTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IProductReadRepository> _productReadRepositoryMock;

    public ChangePriceCommandHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productReadRepositoryMock = new Mock<IProductReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveChangesAsyncCalledOnceAndReturnsTrue()
    {
        //Arrange
        var product = new Product("Suporte p/ celular", 10, 100.5m);

        _productReadRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(product);

        var requestCommand = new ChangePriceCommand(product.Id, 50.5m);

        var handler = new ChangePriceCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        //Assert
        result.ShouldBeTrue();
        _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsFalse()
    {
        //Arrange
        _productReadRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var requestCommand = new ChangePriceCommand(Guid.Parse("6bc099b6-e78d-4fa6-9e3f-58ed7c7a32e7"), 10);

        var handler = new ChangePriceCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        //Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_NonPositivePrice_ThrowsArgumentException()
    {
        //Arrange
        var product = new Product("Phone", 10, 50);

        _productReadRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var requestCommand = new ChangePriceCommand(product.Id, -5);

        var handler = new ChangePriceCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act & Assert
        var exception = await Should.ThrowAsync<ArgumentException>(() => handler.Handle(requestCommand, CancellationToken.None));
        exception.Message.ShouldContain("O preço deve ser maior que zero");
    }

    [Fact]
    public async Task Handle_EnteringTheSameValue_ThrowsArgumentException()
    {
        //Arrange
        var product = new Product("Phone", 10, 50);

        _productReadRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var requestCommand = new ChangePriceCommand(product.Id, 50);

        var handler = new ChangePriceCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act & Assert
        var exception = await Should.ThrowAsync<ArgumentException>(() => handler.Handle(requestCommand, CancellationToken.None));
        exception.Message.ShouldContain($"O valor do produto já é {product.Price}");
    }
}
