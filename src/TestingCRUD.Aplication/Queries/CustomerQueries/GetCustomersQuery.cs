using MediatR;

using TestingCRUD.Aplication.ViewModels.CustomerViewModels;


namespace TestingCRUD.Aplication.Queries.CustomerQueries
{
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>
    {

    }
}
