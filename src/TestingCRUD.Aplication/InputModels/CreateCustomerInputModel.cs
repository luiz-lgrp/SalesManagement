namespace TestingCRUD.Aplication.InputModels
{
    public class CreateCustomerInputModel
    {
        public string Name { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
