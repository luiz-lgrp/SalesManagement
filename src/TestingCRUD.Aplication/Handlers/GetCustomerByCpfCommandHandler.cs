using TestingCRUD.Aplication.ViewModels;
using TestingCRUD.Aplication.Queries;
using TestingCRUD.Domain.Repositories;

using MediatR;

namespace TestingCRUD.Aplication.Handlers
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
                Birthday = customer.Birthday,
                Phone = customer.Phone,
                Status = customer.Status
            };

            return customerVM;
        }
    }
}
