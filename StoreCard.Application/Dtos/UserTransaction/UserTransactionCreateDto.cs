namespace StoreCard.Application.Dtos.UserTransaction
{
    public class UserTransactionCreateDto
    {
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;

    }
}
