namespace StoreCard.Domain.Entities
{
    public class UserTransaction : BaseEntity
    {
        public int UserId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }


        public User User { get; set; } = default!;
    }
}
