namespace ProjectMicroservices.Model;

public class GenreMovie
{
    public int GenreId { get; set; }
    public Genre Genre { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
}