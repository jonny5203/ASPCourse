using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Genre : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Movie> Movies { get; } = [];
}