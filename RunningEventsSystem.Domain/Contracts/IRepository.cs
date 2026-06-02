using System.Linq.Expressions;

namespace RunningEventsSystem.Domain.Contracts;

public interface IRepository<TEntity> where TEntity : class
{
    int Count();
    TEntity? Get(int id);
    IList<TEntity> GetAll();
    IList<TEntity> Find(Expression<Func<TEntity, bool>> expression);

    void Insert(TEntity entity);
    void Delete(TEntity entity);
}
