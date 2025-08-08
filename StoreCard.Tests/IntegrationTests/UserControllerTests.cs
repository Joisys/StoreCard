using StoreCard.Application.Dtos.User;
using System.Net;
using System.Net.Http.Json;

namespace StoreCard.Tests.IntegrationTests
{

    [TestFixture]
    public class UserControllerTests
    {
        private StoreCardWebApplicationFactory _factory = null!;
        private HttpClient _client = null!;

        [SetUp]
        public void SetUp()
        {
            _factory = new StoreCardWebApplicationFactory();
            _client = _factory.CreateClient();
        }

        [TearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkAndData()
        {
            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            Assert.That(users, Is.Not.Null);
            Assert.That(users!.Count, Is.GreaterThan(0));
        }

        [Test]
        public async Task CreateUser_ReturnsCreated()
        {
            var newUser = new UserCreateDto
            {
                FullName = "New User Name",
            };

            var response = await _client.PostAsJsonAsync("/api/users", newUser);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();
            Assert.That(createdUser, Is.Not.Null);
            Assert.That(createdUser!.FullName, Is.EqualTo("New User Name"));
        }

    }
}
