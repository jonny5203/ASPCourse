using System.ComponentModel.DataAnnotations;

namespace ProjectMicroservices.Model.BaseModel;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}