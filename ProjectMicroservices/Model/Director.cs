using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Director : BaseEntity
{
    public string Name { get; set; }
    public int Age { get; set; }
    public ICollection<Movie> Movies { get; } = [];
}