using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.CustomerCommands;
using TestingCRUD.Application.Validations.CustomerCommandValidation;

namespace TestingCRUD.Application.Handlers.CustomerHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, bool>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReadRepository _customerReadRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, ICustomerReadRepository customerReadRepository)
        {
            _customerRepository = customerRepository;
            _customerReadRepository = customerReadRepository;
        }

        public async Task<bool> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var updateModel = request.UpdateCustomer;

            var validationResult = new UpdateCustomerValidator().Validate(updateModel);

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
}
