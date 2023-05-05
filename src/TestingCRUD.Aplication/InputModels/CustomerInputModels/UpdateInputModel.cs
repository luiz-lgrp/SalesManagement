using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Application.CustomerInputModels
{
    //TODO: tirar classe desnecessária
    public class UpdateInputModel
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
