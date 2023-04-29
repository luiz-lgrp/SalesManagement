using MediatR;
using FluentValidation;

using TestingCRUD.Domain.Models;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Commands.CustomerCommands;
using TestingCRUD.Aplication.Validations.CustomerCommandValidation;

namespace TestingCRUD.Aplication.Handlers.CustomerHandlers
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
                createModel.Email,
                createModel.Phone);

            var createdCustomer = await _customerRepository.CreateAsync(customer, cancellationToken);

            return createdCustomer;
        }
    }
}
