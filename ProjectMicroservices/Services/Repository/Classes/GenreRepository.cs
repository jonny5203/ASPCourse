using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    private readonly MovieDbContext _dbContext;
    public GenreRepository(MovieDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}