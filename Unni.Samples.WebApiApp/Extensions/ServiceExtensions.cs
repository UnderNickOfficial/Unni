using Microsoft.EntityFrameworkCore;
using System.Data;
using Unni.Infrastructure.Database.Repositories;
using Unni.Infrastructure.Database.Repositories.Interfaces;
using Unni.Infrastructure.Logger.Services;
using Unni.Infrastructure.Logger.Services.Interfaces;
using Unni.Samples.WebApiApp.Contexts;

namespace Unni.Samples.WebApiApp.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseLoggerService<UnniDbContext>, DatabaseLoggerService<UnniDbContext>>();
            return services;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextFactory<UnniDbContext>(options =>
                options
                .UseSqlServer(configuration.GetConnectionString("UnniDbConnection")));
            GenericRepositorySettings.AddSupportedDbContextType<UnniDbContext>();
            return services;
        }

        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork<UnniDbContext>, UnitOfWork<UnniDbContext>>();
            return services;
        }
    }
}
