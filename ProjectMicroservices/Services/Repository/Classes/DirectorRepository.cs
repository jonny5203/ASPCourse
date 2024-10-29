using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class DirectorRepository : Repository<Director>, IDirectorRepository
{
    private readonly MovieDbContext _dbContext;
    public DirectorRepository(MovieDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}