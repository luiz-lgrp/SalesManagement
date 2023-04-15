using TestingCRUD.Domain.ViewModels;

using MediatR;

namespace TestingCRUD.Aplication.Queries
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
