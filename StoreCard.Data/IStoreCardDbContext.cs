using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace StoreCard.Data
{
    public interface IStoreCardDbContext : IDisposable
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }

        EntityEntry Entry([NotNull] object entity);
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();
        void MigrateDatabase();

    }
}
