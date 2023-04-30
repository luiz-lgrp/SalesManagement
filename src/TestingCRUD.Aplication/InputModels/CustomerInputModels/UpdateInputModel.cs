using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Aplication.CustomerInputModels
{
    public class UpdateInputModel
    {//TODO: criar metodo para alterar CPF
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
