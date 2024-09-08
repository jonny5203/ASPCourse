using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Review : BaseEntity
{
    public override int Id { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}