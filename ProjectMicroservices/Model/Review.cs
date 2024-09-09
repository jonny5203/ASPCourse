using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Review : BaseEntity
{
    public string Content { get; set; }
    public int Rating { get; set; }
    
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}