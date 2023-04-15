using TestingCRUD.Domain.Models;
using TestingCRUD.Aplication.Commands;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Validations;

using MediatR;
using FluentValidation;

namespace TestingCRUD.Aplication.Handlers
{
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;

        public CreateCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
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
                createModel.Birthday,
                createModel.Phone);

            var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

            return createdCustomer;
        }
    }
}
