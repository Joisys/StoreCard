using StoreCard.Domain.Entities;
using System.Linq.Expressions;

namespace StoreCard.Data.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User entity);
        Task<User> UpdateUserAsync(User entity);
        Task RemoveUserAsync(User entity);

        Task<IEnumerable<User>> GetUsersList(Func<IQueryable<User>, IQueryable<User>> include = null!);
        Task<User?> GetQueryById(int id, IEnumerable<Expression<Func<User, object>>>? includes = null,
                    bool asNoTracking = false);

    }
}
