using FluentValidation;
using MediatR;
using TestingCRUD.Aplication.InputModels;
using TestingCRUD.Aplication.Shared;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Result<CustomerViewModel>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IValidator<CreateCustomerInputModel> _validator;

    public CreateCustomerCommandHandler(ICustomerRepository customerRepository, 
        IValidator<CreateCustomerInputModel> validator)
    {
        _customerRepository = customerRepository;
        _validator = validator;
    }

    public async Task<Result<CustomerViewModel>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var createModel = request.CreateCustomer;

        var validationResult = await _validator.ValidateAsync(createModel);

        if (!validationResult.IsValid)
        {
            return Result<CustomerViewModel>.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var customer = new Customer(
            createModel.Name,
            createModel.Cpf,
            createModel.Email,
            createModel.Phone);

        var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

        if (createdCustomer is null)
            return Result<CustomerViewModel>.Fail(new[] { "Erro ao criar cliente.\n" + "Tente novamente." });

        var customerVM = new CustomerViewModel
        {
            Name = createdCustomer.Name,
            Cpf = createdCustomer.Cpf,
            Email = createdCustomer.Email,
            Phone = createdCustomer.Phone,
            Status = createdCustomer.Status,
        };

        return Result<CustomerViewModel>.Ok(customerVM);
    }
}
