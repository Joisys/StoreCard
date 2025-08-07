namespace StoreCard.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStoreCardDbContext Context { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();

        Task<int> SaveChangesAsync();
    }
}
