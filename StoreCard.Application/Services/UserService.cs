using AutoMapper;
using StoreCard.Application.Dtos.User;
using StoreCard.Application.Interfaces;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;

namespace StoreCard.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserAsync(int id)
        {
            var user = await _userRepository.GetQueryById(id);
            if (user == null)
                throw new KeyNotFoundException($"User with Id {id} not found.");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetUsersList();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDto> CreateUserAsync(UserCreateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var user = _mapper.Map<User>(dto);
            await _userRepository.AddUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserAsync(UserUpdateDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var user = await _userRepository.GetQueryById(dto.Id);
            if (user == null)
                throw new KeyNotFoundException($"User with Id {dto.Id} not found.");

            _mapper.Map(dto, user);

            await _userRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetQueryById(id);
            if (user == null)
                return false;

            await _userRepository.RemoveUserAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

    }
}
