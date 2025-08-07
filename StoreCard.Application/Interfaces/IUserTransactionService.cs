using StoreCard.Application.Dtos.User;

namespace StoreCard.Application.Interfaces
{
    public interface IUserTransactionService
    {
        Task<UserDto> GetUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserCreateDto dto);
        Task<UserDto> UpdateUserAsync(UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
