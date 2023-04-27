using TestingCRUD.Aplication.ViewModels;
using TestingCRUD.Aplication.Queries;
using TestingCRUD.Domain.Repositories;

using MediatR;

namespace TestingCRUD.Aplication.Handlers
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
                Birthday = customer.Birthday,
                Phone = customer.Phone,
                Status = customer.Status
            });

            return customersVM;
        }
    }
}
