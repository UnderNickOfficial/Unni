#pragma warning disable CS8600
#pragma warning disable CS8603
using Microsoft.EntityFrameworkCore;
using Unni.Infrastructure.Database.Repositories.Interfaces;

namespace Unni.Infrastructure.Database.Repositories
{
    internal static class GenericRepositoryFactory
    {
        internal static IGenericRepository<TContext, TEntity> NewInstance<TContext, TEntity>(TContext context)
            where TContext : DbContext
            where TEntity : class
        {
            var supportedContextType =
                GenericRepositorySettings.GetSupportedDbContextTypes()
                .FirstOrDefault(supportedContextType => supportedContextType == typeof(TContext));

            if (supportedContextType is not null && supportedContextType == context.GetType())
            {
                var contextTableTypes = context.Model.GetEntityTypes().Select(t => t.ClrType);

                if (contextTableTypes.Contains(typeof(TEntity)))
                {
                    var repositoryType = typeof(GenericRepository<,>);

                    var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TContext), typeof(TEntity)), context);
                    return (IGenericRepository<TContext, TEntity>)repositoryInstance;
                }
            }

            throw new InvalidOperationException($"Unable to instantiate generic repository " +
                                                $"with given: entity type {typeof(TEntity).FullName}, " +
                                                $"context type {typeof(TContext).Name}");
        }
    }
}
