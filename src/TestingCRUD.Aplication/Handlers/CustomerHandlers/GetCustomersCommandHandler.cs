using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Aplication.Queries.CustomerQueries;
using TestingCRUD.Aplication.ViewModels.CustomerViewModels;

namespace TestingCRUD.Aplication.Handlers.CustomerHandlers
{
    public class GetCustomersCommandHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerViewModel>>
    {
        private readonly ICustomerReadRepository _customerReadRepository;

        public GetCustomersCommandHandler(ICustomerReadRepository customerReadRepository)
        {
            _customerReadRepository = customerReadRepository;
        }

        public async Task<IEnumerable<CustomerViewModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerReadRepository.GetAll(cancellationToken);

            if (customers is null)
                return null!;

            var customersVM = customers.Select(customer => new CustomerViewModel
            {
                Name = customer.Name,
                Cpf = customer.Cpf,
                Email = customer.Email,
                Phone = customer.Phone,
                Status = customer.Status,
                Created = customer.Created.ToString(),
                Updated = customer.Updated.ToString()
            });

            return customersVM;
        }
    }
}
