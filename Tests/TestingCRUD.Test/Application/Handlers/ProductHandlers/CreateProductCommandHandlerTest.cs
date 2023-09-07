using TestingCRUD.Application.Validations.ProductCommandValidation;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Test.Application.Handlers.ProductHandlers;

public class CreateProductCommandHandlerTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private Mock<IProductReadRepository> _productReadRepositoryMock;

    public CreateProductCommandHandlerTest()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _productReadRepositoryMock = new Mock<IProductReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsProductViewModel()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "Test",
            Stock = 5,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act
        var result = await handler.Handle(createProductCommand, CancellationToken.None);

        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<ProductViewModel>();
        result.ProductName.ShouldBe(productInputModel.ProductName);
        result.Stock.ShouldBe(productInputModel.Stock);
        result.Price.ShouldBe(productInputModel.Price);
        result.Status.ShouldBe(EntityStatus.Active);
    }

    [Fact]
    public async Task Handle_ValidCommand_ProductIsSavedInRepository()
    {
        // Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "Suporte p/ celular",
            Stock = 10,
            Price = 100.5m
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        _productReadRepositoryMock
            .Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string name, CancellationToken cancellationToken) => null);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product product, CancellationToken cancellationToken) => product);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        // Act
        var result = await handler.Handle(createProductCommand, CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Product>(
            p => p.ProductName == productInputModel.ProductName
                 && p.Stock == productInputModel.Stock
                 && p.Price == productInputModel.Price), CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShortNameInCommand_ThrowsValidatonException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "D3",
            Stock = 5,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode ser menor que 03 caracteres");
    }

    [Fact]
    public async Task Handle_LongNameInCommand_ThrowsValidatonException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "Mega caixa de som d3rfew447 plus",
            Stock = 5,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode passar de 20 caracteres");
    }

    [Fact]
    public async Task Handle_EmptyNameInCommand_ThrowsValidatonException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = " ",
            Stock = 5,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite o nome do produto");
    }

    [Fact]
    public async Task Handle_DuplicateNameInCommand_ThrowsValidationException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "Phone",
            Stock = 5,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("Já existe um produto com este nome");
    }

    [Fact]
    public async Task Handle_StockZeroedInCommand_ThrowsValidationException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "D35x",
            Stock = 0,
            Price = 10,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O valor em estoque deve ser maior que zero");
    }

    [Fact]
    public async Task Handle_PriceZeroedInCommand_ThrowsValidationException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "D35x",
            Stock = 15,
            Price = 0,
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        var createdProduct = new Product(
                    productInputModel.ProductName,
                    productInputModel.Stock,
                    productInputModel.Price);

        _productRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProduct);

        _productReadRepositoryMock
            .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Guid id, CancellationToken CancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O preço do produto deve ser maior que zero");
    }

    [Fact]
    public async Task Handle_StockNotDefinedInCommand_ThrowsValidationException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "D35x",
            // Stock não é definido aqui, então será 0 por padrão.
            Price = 10.5m
        };

        var createProductCommand = new CreateProductCommand(productInputModel);

        _productReadRepositoryMock
            .Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string name, CancellationToken cancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O valor em estoque deve ser maior que zero");
    }

    [Fact]
    public async Task Handle_PriceNotDefinedInCommand_ThrowsValidationException()
    {
        //Arrange
        var productInputModel = new ProductInputModel
        {
            ProductName = "D35x",
            Stock = 10,
        };
        
        var createProductCommand = new CreateProductCommand(productInputModel);

        _productReadRepositoryMock
            .Setup(x => x.GetByName(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string name, CancellationToken cancellationToken) => null);

        var validator = new CreateProductValidator(_productReadRepositoryMock.Object);

        var handler = new CreateProductCommandHandler(_productRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createProductCommand, CancellationToken.None));
        exception.Message.ShouldContain("O preço do produto deve ser maior que zero");
    }


}
