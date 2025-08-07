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
        public DbSet<UserTransaction> UserTransactions => Set<UserTransaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTransaction>()
                        .HasIndex(ut => new { ut.UserId, ut.TransactionDate, ut.Amount, ut.Type })
                        .IsUnique();

        }


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
    }
}
