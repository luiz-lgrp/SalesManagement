using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Commands.CustomerCommands;
using TestingCRUD.Aplication.Validations.CustomerCommandValidation;

namespace TestingCRUD.Aplication.Handlers.CustomerHandlers
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerReadRepository _customerReadRepository;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, ICustomerReadRepository customerReadRepository)
        {
            _customerRepository = customerRepository;
            _customerReadRepository = customerReadRepository;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var updateModel = request.UpdateCustomer;

            var validationResult = new UpdateCustomerValidator().Validate(updateModel);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var customer = await _customerReadRepository.GetByCpf(request.Cpf, cancellationToken);

            if (customer is null)
                return null!;

            customer.Name = updateModel.Name;
            customer.Cpf = updateModel.Cpf;
            customer.Email = updateModel.Email;
            customer.Phone = updateModel.Phone;
            customer.Updated = DateTime.Now;

            var updatedCustomer = await _customerRepository.UpdateAsync(request.Cpf, customer, cancellationToken);

            return updatedCustomer;
        }
    }
}
