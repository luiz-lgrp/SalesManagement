using FluentValidation;
using MediatR;
using TestingCRUD.Aplication.Shared;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.InputModels;
using TestingCRUD.Domain.Repositories;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Result<bool>>
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

    public async Task<Result<bool>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        var updateModel = request.UpdateCustomer;

        var validationResult = await _validator.ValidateAsync(updateModel);

        if (!validationResult.IsValid)
        {
            // Retorna os erros para o front
            return Result<bool>.Fail(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        var customer = await _customerReadRepository.GetByCpf(request.Cpf, cancellationToken);

        if (customer is null)
            return Result<bool>.Fail(new[] { "Cliente não encontrado." });

        customer.Name = updateModel.Name;
        customer.Email = updateModel.Email;
        customer.Phone = updateModel.Phone;
        customer.Updated = DateTime.Now;

        var isUpToDate = await _customerRepository.UpdateAsync(request.Cpf, customer, cancellationToken);

        if (!isUpToDate)
            return Result<bool>.Fail(new[] { "Erro ao atualizar cliente." });

        return Result<bool>.Ok(true);
    }
}
