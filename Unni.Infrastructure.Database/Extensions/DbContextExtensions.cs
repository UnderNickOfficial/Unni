using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unni.Infrastructure.Database.Models.Interfaces;

namespace Unni.Infrastructure.Database.Extensions
{
    public static class DbContextExtensions
    {
        public static void AddAuditableEntity(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType) && entityType.BaseType == null)
                {
                    modelBuilder.SetSoftDeleteFilter(entityType.ClrType);
                }
            }
        }

        public static void AddAuditableEntity(this ChangeTracker changeTracker)
        {
            changeTracker.DetectChanges();

            foreach (var trackedEntity in changeTracker.Entries())
            {
                if (trackedEntity.Entity is IAuditableEntity auditableEntity)
                {
                    if (trackedEntity.State == EntityState.Added)
                    {
                        auditableEntity.CreatedAt = DateTime.UtcNow;
                    }
                    if (trackedEntity.State == EntityState.Modified)
                    {
                        auditableEntity.UpdatedAt = DateTime.UtcNow;
                    }
                    if (trackedEntity.State == EntityState.Deleted)
                    {
                        trackedEntity.State = EntityState.Modified;
                        auditableEntity.DeletedAt = DateTime.UtcNow;
                    }
                }
            }
        }
    }
}
