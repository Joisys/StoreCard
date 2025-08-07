using Microsoft.EntityFrameworkCore;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;
using System.Linq.Expressions;

namespace StoreCard.Data.Repository
{
    public class UserTransactionRepository : IUserTransactionRepository
    {
        private readonly IRepository<UserTransaction> _repository;

        public UserTransactionRepository(IRepository<UserTransaction> repository)
        {
            _repository = repository;
        }

        public async Task<UserTransaction> AddUserTransactionAsync(UserTransaction entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<IEnumerable<UserTransaction>> GetUserTransactionsList(
            Func<IQueryable<UserTransaction>, IQueryable<UserTransaction>>? include = null)
        {
            var query = include != null
                ? include(_repository.GetQueryable())
                : _repository.GetQueryable();

            return await query.ToListAsync();
        }

        public async Task<UserTransaction?> GetQueryById(
            int id,
            IEnumerable<Expression<Func<UserTransaction, object>>>? includes = null,
            bool asNoTracking = false)
        {
            var userTransaction = await _repository
                .GetQueryable(x => x.Id == id, includes, asNoTracking)
                .FirstOrDefaultAsync();

            return userTransaction ?? throw new InvalidOperationException($"UserTransaction with Id {id} not found.");
        }

        public async Task<IEnumerable<UserTransaction>> GetTransactionsByFilterAsync(
            Expression<Func<UserTransaction, bool>> filter,
            Func<IQueryable<UserTransaction>, IQueryable<UserTransaction>>? include = null)
        {
            var query = _repository.GetQueryable();

            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }
    }

}
