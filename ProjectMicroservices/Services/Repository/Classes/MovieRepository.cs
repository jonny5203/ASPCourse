using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class MovieRepository : Repository<Movie>, IMovieRepository
{
    private readonly MovieDbContext _dbContext;
        
    public MovieRepository(MovieDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    
}