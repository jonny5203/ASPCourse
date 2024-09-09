using ProjectMicroservices.Model.BaseModel;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;
using ProjectMicroservices.Services.Repository.Classes;
using ProjectMicroservices.Services.Repository.UnitOfWork.Interface;

namespace ProjectMicroservices.Services.Repository.UnitOfWork.Class;

public class UnitOfWork : IUnitOfWork
{
    private readonly MovieDbContext _dbContext;
    // Dictionary for the Repository Objects
    private Dictionary<Type, object> _repositories;

    public UnitOfWork(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
        _repositories = new Dictionary<Type, object>();
    }
    
    public void Dispose()
    {
        _dbContext.Dispose();
    }

    // 
    public IRepository<T> GetRepository<T>() where T : BaseEntity
    {
        if (_repositories.ContainsKey(typeof(BaseEntity)))
        {
            return (IRepository<T>) _repositories[typeof(BaseEntity)];
        }

        var repository = new Repository<T>(_dbContext);
        _repositories.Add(typeof(T), repository);
        return repository;
    }
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}