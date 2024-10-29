using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectMicroservices.Model.BaseModel;
using ProjectMicroservices.Model.DataAcccess;
using ProjectMicroservices.Services.Repository.Interfaces;

namespace ProjectMicroservices.Services.Repository.Classes;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    private readonly MovieDbContext _dbContext;
    private readonly DbSet<BaseEntity> _dbSet;

    // Getting dbcontext through DI and configure a dbset to work with it, basically creating a
    // dynamic dbset that can work with all the models(tables) instances defined in MovieDbContext.cs
    public Repository(MovieDbContext dbContext)
    {
        _dbContext = dbContext;
        
        _dbSet = _dbContext.Set<BaseEntity>();
    }
    
    public BaseEntity Get(int id)
    {
        IQueryable<BaseEntity> query = _dbSet.Where(n => n.Id == id);
        
        return query.FirstOrDefault();
    }

    public List<BaseEntity> GetAll()
    {
        return _dbSet.ToList();
    }

    public List<BaseEntity> GetList(Expression<Func<BaseEntity, bool>> predicate)
    {
        IQueryable<BaseEntity> query = _dbSet.Where(predicate);
        return query.ToList();
    }

    public void Add(BaseEntity entity)
    {
        _dbSet.Add(entity);
    }

    public BaseEntity? Delete(int id)
    {
        try
        {
            var entity = _dbSet.Find(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                return entity;
            }

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public void Update(BaseEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }

    public bool ifIDExists(int id)
    {
        return _dbSet.Any(n => n.Id == id);
    }
}