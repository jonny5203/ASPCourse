using System.ComponentModel.DataAnnotations;
using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Model;

public class Movie : BaseEntity
{
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; }
    public int? ReleaseYear { get; set; }
    public double? Price { get; set; }

    public ICollection<Genre> Genres { get; } = [];
    public ICollection<Actor> Actors { get; } = [];
    public ICollection<Review> Reviews { get; } = [];

    [Required] public int DirectorId { get; set; }
    public Director Directors { get; }
}