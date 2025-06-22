using Moq;
using TestingCRUD.Aplication.InputModels;
using TestingCRUD.Application.Validations.ProductCommandValidation;

namespace TestingCRUD.Test.Application.Handlers.CustomerHandlers;

public class CreateCustomerCommandHandlerTest
{
    private  Mock<ICustomerRepository> _customerRepositoryMock;
    private  Mock<ICustomerReadRepository> _customerReadRepositoryMock;

    public CreateCustomerCommandHandlerTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerReadRepositoryMock = new Mock<ICustomerReadRepository>();
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsCustomerViewModel()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "John Doe",
            Cpf = "14612697761",
            Email = "johndoe@example.com",
            Phone = "99-99999-9999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

        _customerRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer customer, CancellationToken cancellationToken) =>
            {
                var createdCustomer = new Customer(
                    createCustomerInputModel.Name,
                    createCustomerInputModel.Cpf,
                    createCustomerInputModel.Email,
                    createCustomerInputModel.Phone);

                return createdCustomer;
            });

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act
        var result = await handler.Handle(createCustomerCommand, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Data.ShouldBeOfType<CustomerViewModel>();
        result.Data.Name.ShouldBe(createCustomerInputModel.Name);
        result.Data.Cpf.ShouldBe(createCustomerInputModel.Cpf);
        result.Data.Email.ShouldBe(createCustomerInputModel.Email);
        result.Data.Phone.ShouldBe(createCustomerInputModel.Phone);
        result.Data.Status.ShouldBe(EntityStatus.Active);


        //Assert.NotNull(result);
        //Assert.IsType<CustomerViewModel>(result.Data);
        //Assert.Equal(createCustomerInputModel.Name, result.Data.Name);
        //Assert.Equal(createCustomerInputModel.Cpf, result.Data.Cpf);
        //Assert.Equal(createCustomerInputModel.Email, result.Data.Email);
        //Assert.Equal(createCustomerInputModel.Phone, result.Data.Phone);
        //Assert.Equal(EntityStatus.Active, result.Data.Status);

    }

    [Fact]
    public async Task Handle_ValidCommand_CustomerIsSavedInRepository()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "John Doe",
            Cpf = "14612697761",
            Email = "johndoe@example.com",
            Phone = "99-99999-9999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

        _customerReadRepositoryMock
            .Setup(x => x.GetByCpf(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) => null);

        _customerRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Customer customer, CancellationToken cancellationToken) => customer);

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act
        var result = await handler.Handle(createCustomerCommand, CancellationToken.None);

        // Assert
        _customerRepositoryMock.Verify(repo => repo.CreateAsync(It.Is<Customer>(
            c => c.Name == createCustomerInputModel.Name
            && c.Cpf == createCustomerInputModel.Cpf
            && c.Email == createCustomerInputModel.Email
            && c.Phone == createCustomerInputModel.Phone
            && c.Status == EntityStatus.Active), CancellationToken.None), Times.Once());
    }

    [Fact]
    public async Task Handle_ShortNameInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "ze",
            Cpf = "77788855547",
            Email = "zeMuniz@example.com",
            Phone = "99-99999-9999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode ser menor que 02 caracteres");

        //var exception = await Assert.ThrowsAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        //Assert.Contains("O campo nome não pode ser menor que 02 caracteres", exception.Message);
    }

    [Fact]
    public async Task Handle_DuplicateCpfInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Marcele",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99-99999-9999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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
            .ReturnsAsync((string cpf, CancellationToken cancellationToken) =>
            {
                var CustomerMock = new Customer(
                    createCustomerInputModel.Name,
                    createCustomerInputModel.Cpf,
                    createCustomerInputModel.Email,
                    createCustomerInputModel.Phone);

                return CustomerMock;
            });

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Este Cpf já está cadastrado");
    }

    [Fact]
    public async Task Handle_InvalidEmailInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Marcele",
            Cpf = "77788855547",
            Email = "Marcelez@",
            Phone = "99-99999-9999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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


        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Formato de email inválido");
    }

    [Fact]
    public async Task Handle_InvalidFormatPhoneInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Marcele",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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


        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Formato de telefone inválido xx-xxxxx-xxxx");
    }

    [Fact]
    public async Task Handle_EmptyNameInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um nome");
    }

    [Fact]
    public async Task Handle_EmptyCpfInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Carlos",
            Cpf = "",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite o seu Cpf");
    }

    [Fact]
    public async Task Handle_EmptyEmailInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "",
            Cpf = "77788855547",
            Email = "",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um Email");
    }

    [Fact]
    public async Task Handle_EmptyPhoneInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Lucia",
            Cpf = "77788855547",
            Email = "Lucia@example.com",
            Phone = ""
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Digite um telefone");
    }

    [Fact]
    public async Task Handle_LongNameInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "marcele Gonçalves munis de souza de garcia",
            Cpf = "77788855547",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo nome não pode passar de 30 caracteres");
    }

    [Fact]
    public async Task Handle_LongCpfInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "marcele",
            Cpf = "777888555777",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("O campo Cpf não pode passar de 11 caracteres");
    }


    [Fact]
    public async Task Handle_LongPhoneInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "marcele",
            Cpf = "77788855577",
            Email = "Marcelez@example.com",
            Phone = "999999999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Telefone não pode ter mais de 11 dígitos contando com DDD");
    }

    [Fact]
    public async Task Handle_InvalidCpfInCommand_ThrowsValidationException()
    {
        // Arrange
        var createCustomerInputModel = new CreateCustomerInputModel
        {
            Name = "Marcela",
            Cpf = "777.888.555-66",
            Email = "Marcelez@example.com",
            Phone = "99999999999"
        };

        var createCustomerCommand = new CreateCustomerCommand(createCustomerInputModel);

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

        var validator = new CreateCustomerValidator(_customerReadRepositoryMock.Object);

        var handler = new CreateCustomerCommandHandler(_customerRepositoryMock.Object, validator);

        // Act & Assert
        var exception = await Should.ThrowAsync<ValidationException>(() => handler.Handle(createCustomerCommand, CancellationToken.None));
        exception.Message.ShouldContain("Cpf inválido com pontos,traços ou letras");
    }
}
