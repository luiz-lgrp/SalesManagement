using TestingCRUD.Domain.ViewModels;

using MediatR;

namespace TestingCRUD.Aplication.Queries
{
    public class GetCustomersQuery : IRequest<IEnumerable<CustomerViewModel>>
    {

    }
}
