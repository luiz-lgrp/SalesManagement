using FluentValidation;
using FluentValidation.Results;
using TestingCRUD.Application.Commands.OrderCommands;
using TestingCRUD.Application.Handlers.OrderHandlers;
using TestingCRUD.Application.Validations.OrderCommandValidation;
using TestingCRUD.Application.ViewModels;

namespace TestingCRUD.Test.Application.Handlers.OrderHandlers
{
    public class CreateOrderCommandHandlerTest
    {
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IValidator<OrderInputModel>> _validatorMock;
        private readonly Mock<IProductReadRepository> _productReadRepositoryMock;

        public CreateOrderCommandHandlerTest()
        {
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _validatorMock = new Mock<IValidator<OrderInputModel>>();
            _productReadRepositoryMock = new Mock<IProductReadRepository>();
        }

        [Fact]
        public async Task Handle_ValidCommand_ReturnsOrderViewModel()
        {
            // Arrange
            var orderInputModel = new OrderInputModel
            {
                Cpf = "14625698755",
                Items = new List<OrderItemInputModel>
                {
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "Teste Phone", Quantity = 1 },
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "Teste Capinha Celular", Quantity = 5 },
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "Teste Película", Quantity = 1 }
                }
            };

            var createOrderCommand = new CreateOrderCommand(orderInputModel);

            var products = new List<Product>
            {
                new Product("Teste Phone", 15, 20) { Id = orderInputModel.Items[0].ProductId },
                new Product("Teste Capinha Celular", 8, 10) { Id = orderInputModel.Items[1].ProductId },
                new Product("Teste Película", 18, 9) { Id = orderInputModel.Items[2].ProductId }
            };

            _productReadRepositoryMock
                .Setup(x => x.GetProductsByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _validatorMock
                .Setup(v => v.ValidateAsync(orderInputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var order = new Order(orderInputModel.Cpf);
            foreach (var item in orderInputModel.Items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                order.AddItemToOrder(new OrderItem(item.ProductId, item.ProductName, item.Quantity, product.Price));
            }

            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object);

            // Act
            var result = await handler.Handle(createOrderCommand, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<OrderViewModel>();
            result.Items.Count.ShouldBe(3);
            result.Items[0].ProductName.ShouldBe("Teste Phone");
            result.Items[1].ProductName.ShouldBe("Teste Capinha Celular");
            result.Items[2].ProductName.ShouldBe("Teste Película");
        }

        [Fact]
        public async Task Handle_ProductNotFound_ThrowsValidationException()
        {
            //Arrange
            var orderInputModel = new OrderInputModel
            {
                Cpf = "14625698755",
                Items = new List<OrderItemInputModel>
                {
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = "Teste Phone", Quantity = 1 },
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = "Teste Capinha Celular", Quantity = 5 },
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = "Teste Película", Quantity = 1 }
                }
            };

            var createOrderCommand = new CreateOrderCommand(orderInputModel);

            _productReadRepositoryMock
                .Setup(x => x.GetById(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            var validator = new CreateOrderCommandValidator(null, _productReadRepositoryMock.Object);
            _validatorMock
                .Setup(v => v.ValidateAsync(It.IsAny<OrderInputModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult
                {
                    Errors = { new ValidationFailure("Items[0].ProductId", "Id não encontrado no banco de dados ou o produto está inativado") }
                });

            var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _validatorMock.Object, _productReadRepositoryMock.Object);

            //Act & Assert
            var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createOrderCommand, CancellationToken.None));
            exception.Message.ShouldContain("Id não encontrado no banco de dados ou o produto está inativado");
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            //Arrange
            var orderInputModel = new OrderInputModel
            {
                Cpf = "14625698755",
                Items = new List<OrderItemInputModel>
                {
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = " ", Quantity = 1 },
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = "Teste Capinha Celular", Quantity = 5 },
                    new OrderItemInputModel { ProductId = new Guid(), ProductName = "Teste Película", Quantity = 1 }
                }
            };

            var createOrderCommand = new CreateOrderCommand(orderInputModel);

            _validatorMock
                .Setup(v => v.ValidateAsync(orderInputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult
                {
                    Errors = { new ValidationFailure("Items[0].ProductName", "Digite o nome do produto") }
                });

            var handler = new CreateOrderCommandHandler(_orderRepositoryMock.Object, _validatorMock.Object, _productReadRepositoryMock.Object);

            // Act & Assert
            var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createOrderCommand, CancellationToken.None));
            exception.Message.ShouldContain("Digite o nome do produto");
        }

        [Fact]
        public async Task Handle_ValidCommand_OrderIsSavedInRepository()
        {
            // Arrange
            var orderInputModel = new OrderInputModel
            {
                Cpf = "14612697766",
                Items = new List<OrderItemInputModel>
                {
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "Fone de ouvido", Quantity = 1 },
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "Carregador", Quantity = 5 },
                    new OrderItemInputModel { ProductId = Guid.NewGuid(), ProductName = "capinha", Quantity = 1 }
                }
            };

            var createOrderCommand = new CreateOrderCommand(orderInputModel);

            var products = new List<Product>
            {
                new Product("Fone de ouvido", 132, 8.99m) { Id = orderInputModel.Items[0].ProductId },
                new Product("Carregador", 84, 19.99m) { Id = orderInputModel.Items[1].ProductId },
                new Product("capinha", 5, 11.9m) { Id = orderInputModel.Items[2].ProductId }
            };

            _productReadRepositoryMock
                .Setup(x => x.GetProductsByIds(It.IsAny<List<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);

            _validatorMock
                .Setup(v => v.ValidateAsync(orderInputModel, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            var order = new Order(orderInputModel.Cpf);
            order.AddItemToOrder(new OrderItem(orderInputModel.Items[0].ProductId, "Fone de ouvido", 1, 8.99m));
            order.AddItemToOrder(new OrderItem(orderInputModel.Items[1].ProductId, "Carregador", 5, 19.99m));
            order.AddItemToOrder(new OrderItem(orderInputModel.Items[2].ProductId, "capinha", 1, 11.9m));

            _orderRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(order);

            var handler = new CreateOrderCommandHandler(
                _orderRepositoryMock.Object,
                _validatorMock.Object,
                _productReadRepositoryMock.Object);

            // Act
            var result = await handler.Handle(createOrderCommand, CancellationToken.None);

            // Assert
            _orderRepositoryMock.Verify(repo => repo.CreateAsync(
                It.Is<Order>(o =>
                    o.Cpf == "14612697766" &&
                    o.OrderItems.Count == 3 &&
                    o.OrderItems.Any(i => i.ProductId == orderInputModel.Items[0].ProductId && i.Quantity == 1) &&
                    o.OrderItems.Any(i => i.ProductId == orderInputModel.Items[1].ProductId && i.Quantity == 5) &&
                    o.OrderItems.Any(i => i.ProductId == orderInputModel.Items[2].ProductId && i.Quantity == 1)
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
