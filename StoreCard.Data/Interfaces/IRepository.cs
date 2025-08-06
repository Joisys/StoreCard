using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace StoreCard.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> GetDbset();

        IAsyncEnumerable<T> GetAsync(Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool asNoTracking = false);

        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null,
            IEnumerable<Expression<Func<T, object>>>? includes = null,
            bool asNoTracking = false);


        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(int id);

        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task RemoveAsync(T entity);
    }
}
