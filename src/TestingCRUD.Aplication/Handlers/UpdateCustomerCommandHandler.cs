using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Commands;
using TestingCRUD.Aplication.Validations;

using MediatR;
using FluentValidation;

namespace TestingCRUD.Aplication.Handlers
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
            customer.Birthday = updateModel.Birthday;
            customer.Phone = updateModel.Phone;
            customer.Status = updateModel.Status;

            var updatedCustomer = await _customerRepository.UpdateAsync(request.Cpf, customer, cancellationToken);

            return updatedCustomer;
        }
    }
}
