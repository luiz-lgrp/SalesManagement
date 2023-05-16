using MediatR;

using TestingCRUD.Application.ViewModels.CustomerViewModels;

namespace TestingCRUD.Application.Queries.CustomerQueries;
public class GetCustomerByCpfQuery : IRequest<CustomerViewModel>
{
    public string Cpf { get; set; }

    public GetCustomerByCpfQuery(string cpf) => Cpf = cpf;
}
