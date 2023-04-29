using MediatR;

using TestingCRUD.Aplication.ViewModels.CustomerViewModels;

namespace TestingCRUD.Aplication.Queries.CustomerQueries
{
    public class GetCustomerByCpfQuery : IRequest<CustomerViewModel>
    {
        public string Cpf { get; set; }

        public GetCustomerByCpfQuery(string cpf)
        {
            Cpf = cpf;
        }
    }
}
