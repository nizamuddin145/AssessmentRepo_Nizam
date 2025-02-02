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
    public class DeleteUserServiceTests
    {
        private Mock<IUserRepository> _userRepository;
        private Guid _testId = Guid.NewGuid();
        private DeleteUserService _deleteUserService;

        public DeleteUserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();

            _deleteUserService = new DeleteUserService(_userRepository.Object);
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
        public async Task DeleteUser_HappyPath_ShouldDeleteUser()
        {
            // Arrange
            (UserModel userRequest, User user) = Arrange();
            _userRepository.Setup(x => x.DeleteAsync(user));

            // Act
            await _deleteUserService.DeleteAsync(user);

            // Assert
            _userRepository.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAllUser_HappyPath_ShouldDeleteUser()
        {
            // Arrange
            _userRepository.Setup(x => x.DeleteAllDcoumentAsync());

            // Act
            await _deleteUserService.DeleteAllAsync();

            // Assert
            _userRepository.Verify(x => x.DeleteAllDcoumentAsync(), Times.Once());
        }
    }
}
