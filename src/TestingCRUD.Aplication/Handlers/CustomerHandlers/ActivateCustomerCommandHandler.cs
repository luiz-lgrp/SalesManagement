using MediatR;

using TestingCRUD.Domain.Enums;
using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.CustomerCommands;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class ActivateCustomerCommandHandler : IRequestHandler<ActivateCustomerCommand, bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerReadRepository _customerReadRepository;

    public ActivateCustomerCommandHandler(
        ICustomerRepository customerRepository, 
        ICustomerReadRepository customerReadRepository)
    {
        _customerRepository = customerRepository;
        _customerReadRepository = customerReadRepository;
    }

    public async Task<bool> Handle(ActivateCustomerCommand request, CancellationToken cancellationToken)
    {
        var cpfCustomer = request.Cpf;

        var customer = await _customerReadRepository.GetByCpf(cpfCustomer, cancellationToken);

        if (customer is null)
            return false;

        customer.Active();

        await _customerRepository.SaveChangesAsync();

        return true;
    }
}
