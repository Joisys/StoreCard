namespace StoreCard.Domain.Entities
{
    public class UserTransactionCreateDto
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; } = default!;

    }
}
