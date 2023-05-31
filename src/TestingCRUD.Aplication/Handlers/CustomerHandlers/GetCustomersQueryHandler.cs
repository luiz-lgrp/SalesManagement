using MediatR;

using TestingCRUD.Domain.Repositories;
using TestingCRUD.Application.Queries.CustomerQueries;
using TestingCRUD.Application.ViewModels.CustomerViewModels;
using TestingCRUD.Application.ViewModels.ProductViewModels;

namespace TestingCRUD.Application.Handlers.CustomerHandlers;
public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, IEnumerable<CustomerViewModel>>
{
    private readonly ICustomerReadRepository _customerReadRepository;

    public GetCustomersQueryHandler(ICustomerReadRepository customerReadRepository)
    {
        _customerReadRepository = customerReadRepository;
    }

    public async Task<IEnumerable<CustomerViewModel>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerReadRepository.GetAll(cancellationToken);

        if (customers is null)
            return Enumerable.Empty<CustomerViewModel>();

        var customersVM = customers.Select(customer => new CustomerViewModel
        {
            Name = customer.Name,
            Cpf = customer.Cpf,
            Email = customer.Email,
            Phone = customer.Phone,
            Status = customer. Status
        });

        return customersVM;
    }
}
