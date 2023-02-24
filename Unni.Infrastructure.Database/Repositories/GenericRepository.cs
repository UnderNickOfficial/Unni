using Microsoft.EntityFrameworkCore;
using Unni.Infrastructure.Database.Repositories.Interfaces;

namespace Unni.Infrastructure.Database.Repositories
{
    public class GenericRepository<TContext, TEntity> : IGenericRepository<TContext, TEntity>
        where TContext : DbContext
        where TEntity : class, new()
    {
        protected readonly TContext context;
        public DbSet<TEntity> Table;
        public IQueryable<TEntity> Query
        {
            get => AsNoTracking ? Table.AsNoTracking() : Table.AsQueryable();
        }
        public bool AsNoTracking { get; set; } = false;

        public GenericRepository(IUnitOfWork<TContext> unitOfWork) : this(unitOfWork.Context) { }

        public GenericRepository(TContext context)
        {
            this.context = context;
            Table = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
            => Table.ToList();

        public TEntity? GetById(object id)
            => Table.Find(id);

        public TEntity Insert(TEntity entity)
            => Table.Add(entity).Entity;

        public void Insert(IEnumerable<TEntity> entity)
            => Table.AddRange(entity);
        public TEntity Update(TEntity entity)
        {
            Table.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public void Update(List<TEntity> entity)
        {
            Table.AttachRange(entity);
            entity.ForEach(x => context.Entry(x).State = EntityState.Modified);
        }

        public TEntity Delete(TEntity entity)
            => Table.Remove(entity).Entity;

        public void Delete(IEnumerable<TEntity> entity)
            => Table.RemoveRange(entity);
        /// <summary>
        /// Updates, deletes and adds entities in old list according to new list
        /// </summary>
        /// <param name="oldList">List with old items</param>
        /// <param name="newList">List with new items</param>
        /// <param name="compareMethod">Method for comparing items</param>
        /// <param name="copyMethod">Method for coping from second argument to first</param>
        public void UpdateList(List<TEntity> oldList, List<TEntity> newList, Func<TEntity, TEntity, bool> compareMethod, Action<TEntity, TEntity> copyMethod)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                var current = oldList[i];
                var found = newList.FirstOrDefault(x => compareMethod(x, current));
                if (found == null)
                {
                    oldList.Remove(current);
                    Delete(current);
                }
            }
            foreach (var item in newList)
            {
                var found = oldList.FirstOrDefault(x => compareMethod(x, item));
                if (found == null)
                {
                    oldList.Add(item);
                    Insert(item);
                }
                else
                {
                    copyMethod(found, item);
                    Update(found);
                }
            }
        }
        public void UpdateList(List<TEntity> oldList, List<TEntity> newList, Func<TEntity, TEntity, bool> compareMethod, Func<TEntity, TEntity, bool> compareMethod2, Action<TEntity, TEntity> copyMethod)
        {
            for (int i = 0; i < oldList.Count; i++)
            {
                var current = oldList[i];
                var found = newList.FirstOrDefault(x => compareMethod(x, current));
                if (found == null)
                {
                    oldList.Remove(current);
                    Delete(current);
                }
            }
            foreach (var item in newList)
            {
                var found = oldList.FirstOrDefault(x => compareMethod2(x, item));
                if (found == null)
                {
                    oldList.Add(item);
                    Insert(item);
                }
                else
                {
                    copyMethod(found, item);
                    Update(found);
                }
            }
        }
        /// <summary>
        /// Updates or adds item refernce
        /// </summary>
        /// <param name="item">Item refernce</param>
        /// <param name="copyMethod">Method for copy in item</param>
        public TEntity UpdateItem(TEntity? item, Action<TEntity> copyMethod)
        {
            if (item == null)
            {
                item = new TEntity();
                copyMethod(item);
                Insert(item);
            }
            else
            {
                copyMethod(item);
                Update(item);
            }
            return item;
        }
    }
}
