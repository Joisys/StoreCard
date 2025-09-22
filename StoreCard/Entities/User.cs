namespace StoreCard.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public ICollection<UserTransaction> UserTransactions { get; set; } = new List<UserTransaction>();
    }
}
