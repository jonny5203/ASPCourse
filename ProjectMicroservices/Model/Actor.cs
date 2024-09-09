using System.Collections;
using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Actor : BaseEntity
{
    public string Name { get; set; }
    public int Age { get; set; }

    public ICollection<Movie> Movies { get; } = [];
}