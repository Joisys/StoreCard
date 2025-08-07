using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace StoreCard.Data
{
    public interface IStoreCardDbContext : IDisposable
    {
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }


        Task<int> SaveChangesAsync();
        void MigrateDatabase();

    }
}
