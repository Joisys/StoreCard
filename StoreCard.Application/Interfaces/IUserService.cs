using StoreCard.Application.Dtos.User;

namespace StoreCard.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserAsync(int id);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(UserDto dto);
        Task<UserDto> UpdateUserAsync(UserDto dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
