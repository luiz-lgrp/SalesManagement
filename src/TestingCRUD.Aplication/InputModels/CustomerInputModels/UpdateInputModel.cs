using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Aplication.CustomerInputModels
{
    public class UpdateInputModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EntityStatus Status { get; set; }
    }
}
