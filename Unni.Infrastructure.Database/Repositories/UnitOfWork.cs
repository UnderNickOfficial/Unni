using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Unni.Infrastructure.Database.Models.Interfaces;
using Unni.Infrastructure.Database.Repositories.Interfaces;

namespace Unni.Infrastructure.Database.Repositories
{
    public class UnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : DbContext
    {
        private readonly TContext context;
        private IDbContextTransaction? transaction;
        private bool isDisposed;

        public UnitOfWork(TContext context)
        {
            this.context = context;
            isDisposed = false;
        }

        public UnitOfWork(IServiceScope serviceScope)
            : this(serviceScope.ServiceProvider.GetRequiredService<TContext>())
        {
        }

        public TContext Context => context;

        public IGenericRepository<TContext, TEntity> Repository<TEntity>() where TEntity : class
            => GenericRepositoryFactory.NewInstance<TContext, TEntity>(Context);

        public async Task CreateTransaction()
            => transaction = await context.Database.BeginTransactionAsync();

        public async Task CreateSavepoint(string savepointName)
        {
            if (transaction != null) await transaction.CreateSavepointAsync(savepointName);
        }

        public async Task Commit()
        {
            if (transaction != null) await transaction.CommitAsync();
        }

        public async Task Rollback()
        {
            if (transaction != null) await transaction.RollbackAsync();
        }

        public async Task RollbackToSavepoint(string savepointName)
        {
            if (transaction != null) await transaction.RollbackToSavepointAsync(savepointName);
        }

        public async Task Save()
            => await context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    try
                    {
                        context.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            isDisposed = true;
        }

        /// <summary>
        /// Detaches local entity and attaches transmitted entity
        /// </summary>
        /// <typeparam name="T">Entity type, inherited from IModelId<TKey></typeparam>
        /// <typeparam name="TKey">Entity Id type</typeparam>
        /// <param name="t">Entity</param>
        public void DetachLocal<T, TKey>(T t)
            where T : class, IModelId<TKey> where TKey : struct, IComparable, IComparable<TKey>
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(t.Id));
            if (!(local == null))
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }

        /// <summary>
        /// Detaches entity
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="t">Entity</param>
        public void Detach<T>(T t)
        {
            if (t != null)
            {
                context.Entry(t).State = EntityState.Detached;
            }
        }

        public void DetachAll()
        {
            foreach (var entry in Context.ChangeTracker.Entries())
                entry.State = EntityState.Detached;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
