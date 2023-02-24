using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Unni.Infrastructure.Database.Repositories.Interfaces;

namespace Unni.Infrastructure.Database.Repositories
{
    public class UnitOfWorkFactory<TContext> : IUnitOfWorkFactory<TContext>
        where TContext : DbContext, new()
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        public UnitOfWorkFactory(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public IUnitOfWork<TContext> Create()
        {
            return new UnitOfWork<TContext>(serviceScopeFactory.CreateScope());
        }
    }
}
