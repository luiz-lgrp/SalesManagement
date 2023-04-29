using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Aplication.ViewModels
{
    public class CustomerViewModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public EntityStatus Status { get; set; }
        public string Created { get; set; }
        public string Updated { get; set; }
    }
}
