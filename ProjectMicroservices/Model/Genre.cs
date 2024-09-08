using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Genre : BaseEntity
{
    public override int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Movie> Movies { get; }
}