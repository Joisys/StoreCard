using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;

namespace StoreCard.Application.Services.ServiceFactory
{
    public class HighVolumeTransactionStrategy : ITransactionSummaryStrategy
    {
        private readonly IUserTransactionRepository _repository;
        private readonly decimal _threshold;

        public HighVolumeTransactionStrategy(IUserTransactionRepository repository, decimal threshold)
        {
            _repository = repository;
            _threshold = threshold;
        }

        public async Task<IEnumerable<UserTransactionSummaryDto>> GetSummaryAsync()
        {
            var transactions = await _repository.GetTransactionsByFilterAsync(t => t.Amount > _threshold);

            return transactions
                .GroupBy(t => new { t.UserId, t.Type })
                .Select(g => new UserTransactionSummaryDto
                {
                    UserId = g.Key.UserId,
                    TransactionType = g.Key.Type.ToString(),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();
        }
    }
}
