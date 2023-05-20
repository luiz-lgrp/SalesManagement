using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Application.Validations.CustomerCommandValidation;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerViewModel>
{
    private readonly ICustomerRepository _customerRepository;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerViewModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var createModel = request.CreateCustomer;

        var validationResult = new CreateCustomerValidator().Validate(createModel);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = new Customer(
            createModel.Name,
            createModel.Cpf,
            createModel.Email,
            createModel.Phone);

        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

        if (createdCustomer is null)
            return null;

        var customerVM = new CustomerViewModel
        {
            Name = createdCustomer.Name,
            Cpf = createdCustomer.Cpf,
            Email = createdCustomer.Email,
            Phone = createdCustomer.Phone,
            Status = createdCustomer.Status,
        };

        return customerVM;
    }
}
