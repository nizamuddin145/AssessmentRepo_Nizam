using BusinessEntities;
using Core.Factories;
using Core.Services.Users;
using Data.Repositories.Products;
using Data.Repositories.Users;
using Moq;
using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.Users
{
    public class GetUserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Guid _testId = Guid.NewGuid();
        private GetUserService _getUserService;

        public GetUserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();

            _getUserService = new GetUserService(_userRepository.Object);
        }

        public (UserModel userRequest, User user) Arrange()
        {
            var userRequest = new UserModel
            {
                Name = "John Smith",
                Email = "mark@companya.com",
                Type = UserTypes.Employee,
                Age = 27,
                AnnualSalary = 72000,
                Tags = new[] { "A", "C", "D" }
            };

            var user = new User();
            user.SetName(userRequest.Name);
            user.SetEmail(userRequest.Email);
            user.SetType(userRequest.Type);
            user.SetAge(userRequest.Age);
            user.SetMonthlySalary(userRequest.AnnualSalary);

            return (userRequest, user);
        }

        [Fact]
        public async Task GetUser_HappyPath_ShouldGetUser()
        {
            // Arrange
            (UserModel userRequest, User user) = Arrange();
            _userRepository.Setup(x => x.GetAsync(_testId.ToString())).ReturnsAsync(user);

            // Act
            User userResult = await _getUserService.GetUserAsync(_testId.ToString());

            // Assert
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult, exactMatch: false);

            _userRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetUsers_HappyPath_ShouldGetAllUsers()
        {
            // Arrange
            (UserModel userRequest, User user) = Arrange();

            List<User> users = new List<User> { user };

            _userRepository.Setup(x => x.GetDocumentAsync(null, userRequest.Name, null, null)).ReturnsAsync(users);

            // Act
            IEnumerable<User> userResult = await _getUserService.GetUsersAsync(null, userRequest.Name, null, null);

            // Assert
            Assert.NotNull(userResult);
            Assert.IsType<IEnumerable<User>>(userResult, exactMatch: false);
            Assert.Single(userResult);

            _userRepository.Verify(x => x.GetDocumentAsync(null, It.IsAny<string>(), null, null), Times.Once());
        }
    }
}
