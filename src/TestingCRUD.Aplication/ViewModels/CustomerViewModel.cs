using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Application.ViewModels.CustomerViewModels;
public class CustomerViewModel
{
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set;}
    public string Phone { get; set; }
    public EntityStatus Status { get; set; }
}
