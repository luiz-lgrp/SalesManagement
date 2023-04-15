using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public EntityStatus Status { get; set; }

        public Customer(string name, string cpf, string birthday, string phone)
        {
            Id = Guid.NewGuid();
            Name = name;
            Cpf = cpf;
            Birthday = birthday;
            Phone = phone;
            Status = EntityStatus.Active;
        }
    }
}
