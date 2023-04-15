using TestingCRUD.Aplication.Commands;
using TestingCRUD.Domain.Repositories;

using MediatR;

namespace TestingCRUD.Aplication.Handlers
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
