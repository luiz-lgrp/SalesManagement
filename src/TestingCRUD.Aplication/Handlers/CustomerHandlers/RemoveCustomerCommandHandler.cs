using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.CustomerCommands;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class RemoveCustomerCommandHandler : IRequestHandler<RemoveCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerReadRepository _customerReadRepository;

    public RemoveCustomerCommandHandler(
        ICustomerRepository customerRepository, 
        ICustomerReadRepository customerReadRepository)
    {
        _customerRepository = customerRepository;
        _customerReadRepository = customerReadRepository;
    }

    public async Task<bool> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerReadRepository.GetByCpf(request.Cpf, cancellationToken);

        if (customer is null)
            return false;

        var customerDeleted = await _customerRepository.DeleteAsync(customer.Cpf, cancellationToken);

        return customerDeleted;
    }
}
