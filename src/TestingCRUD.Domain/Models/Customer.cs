namespace TestingCRUD.Domain.Models
{
    public class Customer : BaseModel
    {
        public Guid CustomerId { get; private set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public Customer(string name, string cpf, string email, string phone)
        {
            CustomerId = Guid.NewGuid();
            Name = name;
            Cpf = cpf;
            Email = email;
            Phone = phone;
        }
    }
}
