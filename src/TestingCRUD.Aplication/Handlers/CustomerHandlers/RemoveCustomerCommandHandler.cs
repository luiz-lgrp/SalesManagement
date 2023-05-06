using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Commands.CustomerCommands;

namespace TestingCRUD.Application.Handlers.CustomerHandlers
{
    public class RemoveCustomerCommandHandler : IRequestHandler<RemoveCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;

        public RemoveCustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Unit> Handle(RemoveCustomerCommand request, CancellationToken cancellationToken)
        {
            await _customerRepository.DeleteAsync(request.Cpf, cancellationToken);

            return Unit.Value;
        }
    }
}
