using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models;
public class Customer : BaseModel
{
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public EntityStatus Status { get; private set; }
    public Customer(string name, string cpf, string email, string phone)
    {
        Name = name;
        Cpf = cpf;
        Email = email;
        Phone = phone;
        Status = EntityStatus.Active;
    }

    public void Inactive()
    {
        if (Status == EntityStatus.Inactive)
            throw new ArgumentException("O Status já está inativo");

        Status = Enums.EntityStatus.Inactive;
        Updated = DateTime.Now;
    }

    public void Active()
    {
        if (Status == EntityStatus.Active)
            throw new ArgumentException("O Status já está ativo");

        Status = Enums.EntityStatus.Active;
        Updated = DateTime.Now;
    }
}
