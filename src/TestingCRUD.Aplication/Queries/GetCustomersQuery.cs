using TestingCRUD.Aplication.ViewModels;

using MediatR;

namespace TestingCRUD.Aplication.Queries
{
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>
    {

    }
}
