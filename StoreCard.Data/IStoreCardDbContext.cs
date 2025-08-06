namespace StoreCard.Data
{
    public interface IStoreCardDbContext : IDisposable
    {
        Task<int> SaveChangesAsync();
        void MigrateDatabase();

    }
}
