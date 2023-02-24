using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Data;
using Unni.Infrastructure.Database.Extensions;
using Unni.Infrastructure.Logger.Models;

namespace Unni.Samples.WebApiApp.Contexts
{
    public class UnniDbContext : DbContext
    {
        #region Models

        #region AppException
        public DbSet<AppException> AppExceptions { get; set; } 
        public DbSet<AppExceptionCase> AppExceptionCases { get; set; } 
        #endregion
        #endregion


        public UnniDbContext() { }

        public UnniDbContext(DbContextOptions<UnniDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddAuditableEntity();
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.AddAuditableEntity();

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
