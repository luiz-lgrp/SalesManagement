

namespace TestingCRUD.Test.Application.Handlers.CustomerHandlers
{
    public class CreateCustomerCommandHandlerTest
    {
        //TODO: Teste dando cpf já cadastrado, achar erro
        [Fact]
        public async Task Handle_ValidCommand_ReturnsCustomerViewModel()
        {
            // Arrange
            var customerInputModel = new CustomerInputModel
            {
                Name = "John Doe",
                Cpf = "14612697761",
                Email = "johndoe@example.com",
                Phone = "99-99999-9999"
            };

            var createCustomerCommand = new CreateCustomerCommand(customerInputModel);

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customerReadRepositoryMock = new Mock<ICustomerReadRepository>();

            customerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer customer, CancellationToken cancellationToken) =>
                {
                    var createdCustomer = new Customer(
                        customerInputModel.Name,
                        customerInputModel.Cpf,
                        customerInputModel.Email,
                        customerInputModel.Phone);

                    return createdCustomer;
                });

            customerReadRepositoryMock
                .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string cpf, CancellationToken cancellationToken) =>
                {
                    var customer = new Customer(
                         customerInputModel.Name,
                         customerInputModel.Cpf,
                         customerInputModel.Email,
                         customerInputModel.Phone);

                    return customer;
                });

            var validator = new CreateCustomerValidator(customerReadRepositoryMock.Object);

            var handler = new CreateCustomerCommandHandler(customerRepositoryMock.Object, validator);

            // Act
            var result = await handler.Handle(createCustomerCommand, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<CustomerViewModel>(result);
            Assert.Equal(customerInputModel.Name, result.Name);
            Assert.Equal(customerInputModel.Cpf, result.Cpf);
            Assert.Equal(customerInputModel.Email, result.Email);
            Assert.Equal(customerInputModel.Phone, result.Phone);
            Assert.Equal(EntityStatus.Active, result.Status);
        }

        [Fact]
        public async Task Handle_InvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var customerInputModel = new CustomerInputModel
            {
                Name = "ze",
                Cpf = "77788855547",
                Email = "zeMuniz@example.com",
                Phone = "99-99999-9999"
            };

            var createCustomerCommand = new CreateCustomerCommand(customerInputModel);

            var customerRepositoryMock = new Mock<ICustomerRepository>();
            var customerReadRepositoryMock = new Mock<ICustomerReadRepository>();

            customerRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Customer customer, CancellationToken cancellationToken) =>
                {
                    var createdCustomer = new Customer(
                        customer.Name,
                        customer.Cpf,
                        customer.Email,
                        customer.Phone);

                    return createdCustomer;
                });

            customerReadRepositoryMock
                .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string cpf, CancellationToken cancellationToken) =>
                {
                    var customer = new Customer(
                        customerInputModel.Name,
                        customerInputModel.Cpf,
                        customerInputModel.Email,
                        customerInputModel.Phone);

                    return customer;
                });

            var validator = new CreateCustomerValidator(customerReadRepositoryMock.Object);

            var handler = new CreateCustomerCommandHandler(customerRepositoryMock.Object, validator);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
            Assert.Contains("O campo nome não pode ser menor que 02 caracteres", exception.Message);
        }
    }
}
