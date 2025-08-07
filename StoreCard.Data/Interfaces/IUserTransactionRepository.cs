using StoreCard.Domain.Entities;
using System.Linq.Expressions;

namespace StoreCard.Data.Interfaces
{
    public interface IUserTransactionRepository
    {
        Task<UserTransaction> AddUserTransactionAsync(UserTransaction entity);

        Task<IEnumerable<UserTransaction>> GetUserTransactionsList(Func<IQueryable<UserTransaction>, IQueryable<UserTransaction>> include = null!);

        Task<UserTransaction?> GetQueryById(int id, IEnumerable<Expression<Func<UserTransaction, object>>>? includes = null, bool asNoTracking = false);
        Task<IEnumerable<UserTransaction>> GetTransactionsByFilterAsync(
            Expression<Func<UserTransaction, bool>> filter,
            Func<IQueryable<UserTransaction>, IQueryable<UserTransaction>>? include = null);
    }
}
