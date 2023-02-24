using Microsoft.EntityFrameworkCore;

namespace Unni.Infrastructure.Database.Repositories.Interfaces
{
    public interface IUnitOfWorkFactory<out TContext>
        where TContext : DbContext, new()
    {
        IUnitOfWork<TContext> Create();
    }
}
