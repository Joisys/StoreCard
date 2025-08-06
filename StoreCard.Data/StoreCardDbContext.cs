using Microsoft.EntityFrameworkCore;
using StoreCard.Domain.Entities;

namespace StoreCard.Data
{
    public class StoreCardDbContext : DbContext, IStoreCardDbContext
    {
        public StoreCardDbContext(DbContextOptions<StoreCardDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public void MigrateDatabase()
        {
            if (Database.IsInMemory())
            {
                return;
            }

            Database.Migrate();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        //public DbSet<UserTransaction> Transactions => Set<UserTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
