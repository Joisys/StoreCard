using Microsoft.EntityFrameworkCore.Storage;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;

namespace StoreCard.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IStoreCardDbContext Context { get; }

        private IDbContextTransaction? _transaction;

        public UnitOfWork(IStoreCardDbContext context)
        {
            Context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await Context.Database.BeginTransactionAsync();
            }
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            SetTimestamps();
            return await Context.SaveChangesAsync();
        }

        private void SetTimestamps()
        {
            var entries = Context.ChangeTracker.Entries<BaseEntity>();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            Context.Dispose();
        }
    }
}
