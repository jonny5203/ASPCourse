using ProjectMicroservices.Model;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class ReviewRepository : Repository<Review>, IReviewRepository
{
    private readonly MovieDbContext _dbContext;
    public ReviewRepository(MovieDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    
    
}