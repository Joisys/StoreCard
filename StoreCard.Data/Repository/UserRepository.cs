using Microsoft.EntityFrameworkCore;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;
using System.Linq.Expressions;

namespace StoreCard.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IRepository<User> _repository;

        public UserRepository(IRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<User> AddUserAsync(User entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task RemoveUserAsync(User entity)
        {
            await _repository.RemoveAsync(entity);
        }

        public async Task<User> UpdateUserAsync(User entity)
        {
            return await _repository.UpdateAsync(entity);
        }


        public async Task<User?> GetQueryById(int id, IEnumerable<Expression<Func<User, object>>>? includes = null,
            bool asNoTracking = false)
        {
            var User = await _repository.GetQueryable(x => x.Id == id, includes, asNoTracking)
                .FirstOrDefaultAsync();

            return User ?? throw new InvalidOperationException($"User with Id {id} not found.");
        }

        public async Task<IEnumerable<User>> GetUsersList(Func<IQueryable<User>, IQueryable<User>>? include = null)
        {
            var query = include != null ? include(_repository.GetQueryable()) : _repository.GetQueryable();
            return await query.ToListAsync();
        }

    }
}
