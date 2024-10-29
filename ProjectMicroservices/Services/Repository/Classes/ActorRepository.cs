using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class ActorRepository : Repository<Actor>, IActorRepository
{
    private readonly MovieDbContext _dbContext;
    public ActorRepository(MovieDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}