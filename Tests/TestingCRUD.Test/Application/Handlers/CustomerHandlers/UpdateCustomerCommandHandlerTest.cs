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

        var customer = new Customer("Jhon", "14785236988", "jhon@example.com.br", "88-5522-8874");

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(customer);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act
        var result = await handler.Handle(inputCustomerCommand, CancellationToken.None);

        //Assert
        result.ShouldBeTrue();
        _customerRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()), Times.Once());
    }

    [Fact]
    public async Task Handle_InvalidCpf_ReturnsFalse()
    {
        // Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Jhon",
            Cpf = "14785236912",
            Email = "jhon@example.com.br",
            Phone = "88-5522-8874"
        };

        var inputCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer)null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        // Act
        var result = await handler.Handle(inputCustomerCommand, CancellationToken.None);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task Handle_ShortNameInCommand_ThrowsValidationException()
    {
        // Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "ze",
            Cpf = "77788855547",
            Email = "zeMuniz@example.com",
            Phone = "99-99999-9999"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var existingCustomer = new Customer("ExistingCustomer", "77788855547", "existing@example.com", "99-99999-9999");
        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingCustomer);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode ser menor que 02 caracteres");
    }

    [Fact]
    public async Task Handle_InvalidEmailInCommand_ThrowsValidationException()
    {
        // Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Marcele",
            Cpf = "77788855547",
            Email = "Marcelez@",
            Phone = "99-99999-9999"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);


        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Formato de email inválido");
    }

    [Fact]
    public async Task Handle_InvalidFormatPhoneInCommand_ThrowsValidationException()
    {
        // Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Marcele",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Formato de telefone inválido xx-xxxxx-xxxx");
    }

    [Fact]
    public async Task Handle_EmptyNameInCommand_ThrowsValidationException()
    {
        // Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um nome");
    }

    [Fact]
    public async Task Handle_EmptyCpfInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Maria",
            Cpf = "",
            Email = "Maria@email.com",
            Phone = "21985786958"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
            .Setup(x => x.UpdateAsync(It.IsAny<string>(), It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite o seu Cpf");
    }

    [Fact]
    public async Task Handle_EmptyEmailInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "",
            Cpf = "12345678955",
            Email = "",
            Phone = "21998547899"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Acte & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um Email");
    }

    [Fact]
    public async Task Handle_EmptyPhoneInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "nameTeste",
            Cpf = "15975385246",
            Email = "Test@example.com",
            Phone = ""
        };

        var inputCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act e Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(inputCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um telefone");
    }

    [Fact]
    public async Task Handle_LongNameInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "marcele Gonçalves munis de souza de garcia",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var inputCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act e Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(inputCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode passar de 30 caracteres");
    }

    [Fact]
    public async Task Handle_LongCpfInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Marcos",
            Cpf = "123456789456123",
            Email = "marcos@example.com",
            Phone = "21589645687"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo Cpf não pode passar de 11 caracteres");
    }

    [Fact]
    public async Task Handle_LongPhoneInCommand_ThrowsValidationException()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Carla",
            Cpf = "12345678991",
            Email = "carlinha@example.com",
            Phone = "12345678903216549870"
        };

        var updateCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act e Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(updateCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Telefone não pode ter mais de 11 dígitos contando com DDD");
    }

    [Fact]
    public async Task Handle_InvalidCpfInCommand_ThrowsValidationExcetion()
    {
        //Arrange
        var customerInputModel = new CustomerInputModel
        {
            Name = "Marcela",
            Cpf = "777.888.555-66",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var inputCustomerCommand = new UpdateCustomerCommand(customerInputModel.Cpf, customerInputModel);

        _customerRepositoryMock
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

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new UpdateCustomerValidator();

        var handler = new UpdateCustomerCommandHandler(_customerRepositoryMock.Object, _customerReadRepositoryMock.Object, validator);

        //Act e Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(inputCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Cpf inválido com pontos,traços ou letras");
    }
}
