using TestingCRUD.Domain.Enums;

namespace TestingCRUD.Domain.Models
{
    public abstract class BaseModel
    {
        public EntityStatus Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public BaseModel()
        {
            Status = EntityStatus.Active;
            Created = DateTime.Now;
            Updated = Created;
        }
    }
}
