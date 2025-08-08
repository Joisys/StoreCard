using AutoMapper;
using NSubstitute;
using StoreCard.Application.Dtos.User;
using StoreCard.Application.Services;
using StoreCard.Data.Interfaces;
using StoreCard.Domain.Entities;

namespace StoreCard.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserRepository _userRepository = null!;
        private IUnitOfWork _unitOfWork = null!;
        private IMapper _mapper = null!;
        private UserService _service = null!;

        [SetUp]
        public void SetUp()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();

            _service = new UserService(_userRepository, _unitOfWork, _mapper);
        }

        [Test]
        public async Task GetUserAsync_ExistingUser_ReturnsUserDto()
        {
            var user = new User { Id = 1, FullName = "Test FullName" };
            var userDto = new UserDto { Id = 1, FullName = "Test FullName" };

            _userRepository.GetQueryById(1).Returns(user);
            _mapper.Map<UserDto>(user).Returns(userDto);

            var result = await _service.GetUserAsync(1);

            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.FullName, Is.EqualTo("Test FullName"));
        }


        [Test]
        public void GetUserAsync_UserNotFound_Throws()
        {
            _userRepository.GetQueryById(Arg.Any<int>()).Returns((User?)null);

            var ex = Assert.ThrowsAsync<KeyNotFoundException>(() => _service.GetUserAsync(100));
            Assert.That(ex?.Message, Does.Contain("User with Id 100 not found."));
        }

        [Test]
        public async Task GetAllUsersAsync_ReturnsMappedDtos()
        {
            var users = new List<User> { new User { Id = 1, FullName = "Test FullName" } };
            var userDtos = new List<UserDto> { new UserDto { Id = 1, FullName = "Test FullName" } };

            _userRepository.GetUsersList().Returns(users);
            _mapper.Map<IEnumerable<UserDto>>(users).Returns(userDtos);

            var result = await _service.GetAllUsersAsync();

            Assert.That(result, Has.Exactly(1).Items);
        }

        [Test]
        public async Task CreateUserAsync_ValidInput_CreatesUser()
        {
            var createDto = new UserCreateDto { FullName = "New FullName" };
            var user = new User { Id = 2, FullName = "New" };
            var userDto = new UserDto { Id = 2, FullName = "New FullName" };

            _mapper.Map<User>(createDto).Returns(user);
            _mapper.Map<UserDto>(user).Returns(userDto);

            var result = await _service.CreateUserAsync(createDto);

            await _userRepository.Received(1).AddUserAsync(user);
            await _unitOfWork.Received(1).SaveChangesAsync();
            Assert.That(result.FullName, Is.EqualTo("New FullName"));
        }

        [Test]
        public void CreateUserAsync_NullInput_Throws()
        {
            Assert.ThrowsAsync<ArgumentNullException>(() => _service.CreateUserAsync(null!));
        }

        [Test]
        public async Task UpdateUserAsync_ValidInput_UpdatesUser()
        {
            var dto = new UserUpdateDto { Id = 1, FullName = "Updated FullName" };
            var user = new User { Id = 1, FullName = "Old FullName" };
            var userDto = new UserDto { Id = 1, FullName = "Updated FullName" };

            _userRepository.GetQueryById(dto.Id).Returns(user);
            _mapper.Map(dto, user);
            _mapper.Map<UserDto>(user).Returns(userDto);

            var result = await _service.UpdateUserAsync(dto);

            await _userRepository.Received(1).UpdateUserAsync(user);
            await _unitOfWork.Received(1).SaveChangesAsync();
            Assert.That(result.FullName, Is.EqualTo("Updated FullName"));
        }

        [Test]
        public async Task DeleteUserAsync_NotFound_ReturnsFalse()
        {
            _userRepository.GetQueryById(Arg.Any<int>()).Returns((User?)null);

            var result = await _service.DeleteUserAsync(99);

            Assert.That(result, Is.False);
            await _userRepository.DidNotReceive().RemoveUserAsync(Arg.Any<User>());
        }

        [TearDown]
        public void TearDown()
        {
            if (_unitOfWork is IDisposable disposableUnitOfWork)
            {
                disposableUnitOfWork.Dispose();
            }
        }

    }
}
