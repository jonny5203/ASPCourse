using System.Linq.Expressions;
using ProjectMicroservices.Model.BaseModel;

namespace ProjectMicroservices.Services.Repository.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    BaseEntity Get(int id);
    List<BaseEntity> GetAll();
    List<BaseEntity> GetList(Expression<Func<BaseEntity, bool>> predicate);
    void Add(BaseEntity entity);
    BaseEntity? Delete(int id);
    void Update(BaseEntity entity);
    void SaveChanges();
}