using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using StoreCard.Data.Interfaces;
using System.Linq.Expressions;

namespace StoreCard.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbSet<T> _dbSet;
        protected readonly IStoreCardDbContext _context;

        public Repository(IStoreCardDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = context.Set<T>();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>>? filter = null, IEnumerable<Expression<Func<T, object>>>? includes = null, bool asNoTracking = false)
        {
            var dbset = GetQueryable(filter, includes, asNoTracking);
            return dbset.AsAsyncEnumerable();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} with ID {id} not found.");
            }
            return entity!;
        }

        public DbSet<T> GetDbset()
        {
            return _dbSet;
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, IEnumerable<Expression<Func<T, object>>>? includes = null, bool asNoTracking = false)
        {
            var dbset = _dbSet.AsQueryable();

            if (filter != null)
            {
                dbset = dbset.Where(filter);
            }

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    dbset = dbset.Include(include);
                }
            }

            if (asNoTracking)
            {
                dbset = dbset.AsNoTracking();
            }

            return dbset;
        }

        public Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task<T> UpdateAsync(T entity)
        {
            return Task.FromResult(Update(entity));
        }

        private T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }
    }
}
