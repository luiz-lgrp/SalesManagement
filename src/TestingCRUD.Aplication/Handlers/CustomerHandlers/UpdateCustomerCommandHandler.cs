using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Application.Commands.CustomerCommands;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerReadRepository _customerReadRepository;
    private readonly IValidator<CustomerInputModel> _validator;

    public UpdateCustomerCommandHandler(
        ICustomerRepository customerRepository, 
        ICustomerReadRepository customerReadRepository,
        IValidator<CustomerInputModel> validator)
    {
        _customerRepository = customerRepository;
        _customerReadRepository = customerReadRepository;
        _validator = validator;
    }

    public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var updateModel = request.UpdateCustomer;

        var validationResult = await _validator.ValidateAsync(updateModel);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var customer = await _customerReadRepository.GetByCpf(request.Cpf, cancellationToken);
        
        if (customer is null)
            return false;

        customer.Name = updateModel.Name;
        customer.Cpf = updateModel.Cpf;
        customer.Email = updateModel.Email;
        customer.Phone = updateModel.Phone;
        customer.Updated = DateTime.Now;

        var IsUpToDate = await _customerRepository.UpdateAsync(request.Cpf, customer, cancellationToken);
        
        return IsUpToDate;
    }
}
