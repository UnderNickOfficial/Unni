using Microsoft.EntityFrameworkCore;

namespace Unni.Infrastructure.Database.Repositories.Interfaces
{
    public interface IGenericRepository<out TContext, TEntity>
        where TContext : DbContext
        where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        bool AsNoTracking { get; set; }
        IQueryable<TEntity> Query { get; }
        TEntity? GetById(object id);
        TEntity Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entity);
        TEntity Update(TEntity entity);
        void Update(List<TEntity> entity);
        TEntity Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entity);
        void UpdateList(List<TEntity> oldList, List<TEntity> newList, Func<TEntity, TEntity, bool> compareMethod, Action<TEntity, TEntity> copyMethod);
        void UpdateList(List<TEntity> oldList, List<TEntity> newList, Func<TEntity, TEntity, bool> compareMethod, Func<TEntity, TEntity, bool> compareMethod2, Action<TEntity, TEntity> copyMethod);
        TEntity UpdateItem(TEntity? item, Action<TEntity> copyMethod);

    }
}
