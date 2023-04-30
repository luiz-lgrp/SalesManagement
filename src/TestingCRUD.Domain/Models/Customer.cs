using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models
{
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
    }
}
