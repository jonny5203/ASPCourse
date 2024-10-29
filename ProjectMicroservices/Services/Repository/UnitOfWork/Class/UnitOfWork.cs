using ProjectMicroservices.Model.BaseModel;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;

namespace ProjectMicroservices.Services.Repository.UnitOfWork.Class;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieDbContext _dbContext;
    
    public IActorRepository Actors { get; }
    public IDirectorRepository Directors { get; }
    public IGenreRepository Genres { get; }
    public IMovieRepository Movies { get; }
    public IReviewRepository Reviews { get; }

    public UnitOfWork(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
        Actors = new ActorRepository(_dbContext);
        Directors = new DirectorRepository(_dbContext);
        Genres = new GenreRepository(_dbContext);
        Movies = new MovieRepository(_dbContext);
        Reviews = new ReviewRepository(_dbContext);
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }
    
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}