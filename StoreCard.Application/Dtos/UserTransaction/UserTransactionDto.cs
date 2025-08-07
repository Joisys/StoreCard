namespace StoreCard.Domain.Entities
{
    public class UserTransactionDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;

    }
}
