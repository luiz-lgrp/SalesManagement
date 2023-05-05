using MediatR;

using TestingCRUD.Application.ViewModels.CustomerViewModels;


namespace TestingCRUD.Application.Queries.CustomerQueries
{
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>
    {

    }
}
