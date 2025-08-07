using AutoMapper;
using Microsoft.Extensions.Logging;
using StoreCard.Application.Dtos.UserTransaction;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;
using StoreCard.Domain.Enums;
using System.Linq.Expressions;

namespace StoreCard.Application.Services
{
    public class UserTransactionService : IUserTransactionService
    {
        private readonly IUserTransactionRepository _userTransactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserTransactionService> _logger;

        public UserTransactionService(
            IUserTransactionRepository transactionRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<UserTransactionService> logger)
        {
            _userTransactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<UserTransactionDto>> GetAllUserTransactionsAsync()
        {
            var transactions = await _userTransactionRepository.GetUserTransactionsList();
            return _mapper.Map<IEnumerable<UserTransactionDto>>(transactions);
        }

        public async Task<UserTransactionDto> GetUserTransactionAsync(int id)
        {
            var transaction = await _userTransactionRepository.GetQueryById(id);
            if (transaction == null)
            {
                _logger.LogWarning("Transaction with Id {TransactionId} not found.", id);
                throw new KeyNotFoundException($"Transaction with Id {id} not found.");
            }

            return _mapper.Map<UserTransactionDto>(transaction);
        }

        public async Task<UserTransactionDto> CreateUserTransactionAsync(UserTransactionCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var transaction = _mapper.Map<UserTransaction>(dto);
            await _userTransactionRepository.AddUserTransactionAsync(transaction);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Created new transaction with Id {TransactionId}", transaction.Id);

            return _mapper.Map<UserTransactionDto>(transaction);
        }

        public async Task<IEnumerable<UserTransactionSummaryDto>> GetUserTransactionSummaryAsync(
            int? userId = null, string? transactionType = null, decimal? threshold = 0)
        {
            ValidateParameters(userId, transactionType, threshold, out TransactionType? parsedType);

            Expression<Func<UserTransaction, bool>> filter = t =>
                (!threshold.HasValue || t.Amount > threshold.Value) &&
                (!parsedType.HasValue || t.Type == parsedType.Value) &&
                (!userId.HasValue || t.UserId == userId.Value);

            var transactions = await _userTransactionRepository.GetTransactionsByFilterAsync(filter);

            var summary = transactions
                .GroupBy(t => new { t.UserId, t.Type })
                .Select(g => new UserTransactionSummaryDto
                {
                    UserId = g.Key.UserId,
                    TransactionType = g.Key.Type.ToString(),
                    TotalAmount = g.Sum(t => t.Amount)
                })
                .ToList();

            return summary;
        }

        private void ValidateParameters(
            int? userId,
            string? transactionType,
            decimal? threshold,
            out TransactionType? parsedType)
        {
            parsedType = null;

            if (userId.HasValue && userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId), "User Id must be a positive integer.");
            }

            if (threshold.HasValue && threshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(threshold), "Threshold must be non-negative.");
            }

            if (!string.IsNullOrWhiteSpace(transactionType))
            {
                if (Enum.TryParse<TransactionType>(transactionType, true, out var typeEnum))
                {
                    parsedType = typeEnum;
                }
                else
                {
                    _logger.LogWarning("Invalid transaction type provided: {TransactionType}", transactionType);
                    throw new ArgumentException("Invalid transaction type.", nameof(transactionType));
                }
            }
        }

    }

}
