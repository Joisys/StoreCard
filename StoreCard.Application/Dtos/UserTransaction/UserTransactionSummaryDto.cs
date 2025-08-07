namespace StoreCard.Application.Dtos.UserTransaction
{
    public class UserTransactionSummaryDto
    {
        public int UserId { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
    }
}
