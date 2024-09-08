using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservices.Model.BaseModel;

public class BaseEntity
{
    [Key]
    public virtual int? Id { get; set; }
}