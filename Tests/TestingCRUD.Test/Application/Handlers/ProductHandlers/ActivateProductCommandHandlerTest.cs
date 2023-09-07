

using TestingCRUD.Domain.Models;

namespace TestingCRUD.Test.Application.Handlers.ProductHandlers;

public class ActivateProductCommandHandlerTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IProductReadRepository> _productReadRepositoryMock;

    public ActivateProductCommandHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productReadRepositoryMock = new Mock<IProductReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsTrueAndProductStatusIsActive()
    {
        //Arrange
        var product = new Product("Fone", 2, 9.99m);

        product.Inactive();

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var requestComand = new ActivateProductCommand(product.Id);

        var handler = new ActivateProductCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestComand, CancellationToken.None);

        //Assert
        result.ShouldBeTrue();
        product.Status.ShouldBe(EntityStatus.Active);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsFalse()
    {
        //Arrange
        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product)null);

        var requestComand = new ActivateProductCommand(Guid.Parse("35eaf68d-0420-4e28-aeee-27d97ad6b6ab"));

        var handler = new ActivateProductCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestComand, CancellationToken.None);

        //Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_ValidCommand_SaveChangeAsyncIsCalledOnce()
    {
        //Arrange
        var product = new Product("Test product", 10, 5);

        product.Inactive();

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var requestCommand = new ActivateProductCommand(product.Id);

        var handler = new ActivateProductCommandHandler(_productRepositoryMock.Object, _productReadRepositoryMock.Object);

        //Act
        var result = await handler.Handle(requestCommand, CancellationToken.None);

        //Assert
        _productRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
