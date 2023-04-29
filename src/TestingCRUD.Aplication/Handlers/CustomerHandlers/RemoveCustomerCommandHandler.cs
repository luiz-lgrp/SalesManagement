using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Commands.CustomerCommands;

namespace TestingCRUD.Aplication.Handlers.CustomerHandlers
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
