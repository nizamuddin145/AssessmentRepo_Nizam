using BusinessEntities;
using Core.Factories;
using Core.Services.Users;
using Data.Repositories.Products;
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
    public class CreateUserServiceTests
    {
        private Mock<IIdObjectFactory<User>> _factoryMock;
        private Mock<IUpdateUserService> _updateUserServiceMock;
        private Guid _testId = Guid.NewGuid();
        private CreateUserService _createUserService;

        public CreateUserServiceTests()
        {
            _factoryMock = new Mock<IIdObjectFactory<User>>();
            _updateUserServiceMock = new Mock<IUpdateUserService>();

            _createUserService = new CreateUserService(_factoryMock.Object, _updateUserServiceMock.Object);
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
        public async Task CreateUser_HappyPath_ShouldCreateAndReturnUser()
        {
            // Arrange
            (UserModel userRequest, User user) = Arrange();
            _factoryMock.Setup(x => x.Create(_testId)).Returns(user);
            _updateUserServiceMock.Setup(x => x.UpdateAsync(user, userRequest));

            // Act
            User userResult = await _createUserService.CreateAsync(_testId, userRequest);

            // Assert
            Assert.NotNull(userResult);
            Assert.IsType<User>(userResult, exactMatch: false);
            Assert.Equal(userResult.Name, userRequest.Name);
            Assert.Equal(userResult.Email, userRequest.Email);
            userResult.Type.Equals(userRequest.Type);
            Assert.Equal(userResult.Age, userRequest.Age);
            Assert.Equal(userResult.MonthlySalary, userRequest.AnnualSalary / 12);

            _updateUserServiceMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<UserModel>()), Times.Once());
        }
    }
}
