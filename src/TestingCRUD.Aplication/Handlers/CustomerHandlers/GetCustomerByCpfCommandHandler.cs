using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Queries.CustomerQueries;
using TestingCRUD.Aplication.ViewModels.CustomerViewModels;

namespace TestingCRUD.Aplication.Handlers.CustomerHandlers
{
    public class GetCustomerByCpfCommandHandler : IRequestHandler<GetCustomerByCpfQuery, CustomerViewModel>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomerByCpfCommandHandler(ICustomerReadRepository customerReadRepository)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<CustomerViewModel> Handle(GetCustomerByCpfQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerReadRepository.GetByCpf(request.Cpf, cancellationToken);

            if (customer is null)
                return null!;

            var customerVM = new CustomerViewModel
            {
                Name = customer.Name,
                Cpf = customer.Cpf,
                Email = customer.Email,
                Phone = customer.Phone,
                Status = customer.Status,
                Created = customer.Created.ToString(),
                Updated = customer.Updated.ToString()
            };

            return customerVM;
        }
    }
}
