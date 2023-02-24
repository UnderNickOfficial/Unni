using Microsoft.EntityFrameworkCore;
using Unni.Infrastructure.Database.Models.Interfaces;

namespace Unni.Infrastructure.Database.Repositories.Interfaces
{
    public interface IUnitOfWork<out TContext> : IDisposable
       where TContext : DbContext
    {
        TContext Context { get; }
        IGenericRepository<TContext, TEntity> Repository<TEntity>() where TEntity : class;
        Task CreateTransaction();
        Task CreateSavepoint(string savepointName);
        Task Commit();
        Task Rollback();
        Task Save();
        Task RollbackToSavepoint(string savepointName);
        void DetachLocal<T, TKey>(T t) where T : class, IModelId<TKey> where TKey : struct, IComparable, IComparable<TKey>;
        public void Detach<T>(T t);
        public void DetachAll();
    }
}
