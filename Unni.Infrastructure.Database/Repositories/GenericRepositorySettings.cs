using Microsoft.EntityFrameworkCore;

namespace Unni.Infrastructure.Database.Repositories
{
    public static class GenericRepositorySettings
    {
        private static List<Type> supportedContexts = new List<Type>();

        public static void AddSupportedDbContextType<TDbContext>() where TDbContext : DbContext
        {
            if (!supportedContexts.Contains(typeof(TDbContext)))
                supportedContexts.Add(typeof(TDbContext));
        }

        public static IEnumerable<Type> GetSupportedDbContextTypes()
            => supportedContexts;
    }
}
