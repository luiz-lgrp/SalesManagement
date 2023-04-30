using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; private set; }
        public DateTime Updated { get; set; }

        protected BaseModel()
        {
            Id = Guid.NewGuid();
            Created = DateTime.Now;
            Updated = Created;
        }
    }
}
