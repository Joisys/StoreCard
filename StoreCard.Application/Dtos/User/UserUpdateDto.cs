namespace StoreCard.Application.Dtos.User
{
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = default!;
    }
}
