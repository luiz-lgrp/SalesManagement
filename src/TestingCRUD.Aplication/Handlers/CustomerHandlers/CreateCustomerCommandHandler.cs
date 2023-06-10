using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.ViewModels.CustomerViewModels;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerViewModel>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CustomerInputModel> _validator;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, 
        IValidator<CustomerInputModel> validator)
    {
        _customerRepository = customerRepository;
        _validator = validator;
    }

    public async Task<CustomerViewModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var createModel = request.CreateCustomer;

        var validationResult = await _validator.ValidateAsync(createModel);

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
