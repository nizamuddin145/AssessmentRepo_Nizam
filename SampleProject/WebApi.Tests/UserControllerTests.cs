using BusinessEntities;
using Core.Services.Users;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.Linq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests
{
    public class UserControllerTests
    {
        private Mock<ILogger<UserController>> _loggerMock;
        private Mock<ICreateUserService> _createUserServiceMock;
        private Mock<IDeleteUserService> _deleteUserServiceMock;
        private Mock<IGetUserService> _getUserServiceMock;
        private Mock<IUpdateUserService> _updateUserServiceMock;
        private Guid _testId = Guid.NewGuid();
        private UserController _userController;

        public UserControllerTests()
        {
            _loggerMock = new Mock<ILogger<UserController>>();
            _createUserServiceMock = new Mock<ICreateUserService>();
            _deleteUserServiceMock = new Mock<IDeleteUserService>();
            _getUserServiceMock = new Mock<IGetUserService>();
            _updateUserServiceMock = new Mock<IUpdateUserService>();

            _userController = new UserController(_loggerMock.Object, _createUserServiceMock.Object, _deleteUserServiceMock.Object, _getUserServiceMock.Object, _updateUserServiceMock.Object);
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
            _createUserServiceMock.Setup(x => x.CreateAsync(_testId, userRequest)).ReturnsAsync(user);

            // Act
            IHttpActionResult userResponse = await _userController.CreateUser(_testId, userRequest);

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as CreatedAtRouteNegotiatedContentResult<UserData>;
            var userResult = result.Content;
            Assert.IsType<CreatedAtRouteNegotiatedContentResult<UserData>>(result, exactMatch: false);
            Assert.Equal(userResult.Name,userRequest.Name);
            Assert.Equal(userResult.Email, userRequest.Email);
            userResult.Type.Equals(userRequest.Type);
            Assert.Equal(userResult.Age, userRequest.Age);
            Assert.Equal(userResult.MonthlySalary, userRequest.AnnualSalary / 12);

            _createUserServiceMock.Verify(x => x.CreateAsync(It.IsAny<Guid>(), It.IsAny<UserModel>()), Times.Once());
        }

        [Fact]
        public async Task UpdateUser_HappyPath_ShouldUpdateAndReturnUpdatedUser()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            _createUserServiceMock.Setup(x => x.CreateAsync(_testId, userRequest)).ReturnsAsync(user);
            _getUserServiceMock.Setup(x => x.GetUserAsync(_testId.ToString())).ReturnsAsync(user);
            userRequest.Email = "test@test.com";
            user.SetEmail(userRequest.Email);
            _updateUserServiceMock.Setup(x => x.UpdateAsync(user, userRequest));

            // Act
            IHttpActionResult userResponse = await _userController.UpdateUser(_testId, userRequest);

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkNegotiatedContentResult<UserData>;
            var userResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<UserData>>(result, exactMatch: false);
            Assert.Equal(userResult.Email, userRequest.Email);

            _updateUserServiceMock.Verify(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<UserModel>()), Times.Once());
            _getUserServiceMock.Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteUser_HappyPath_ShouldDeleteUser()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            _getUserServiceMock.Setup(x => x.GetUserAsync(_testId.ToString())).ReturnsAsync(user);
            _deleteUserServiceMock.Setup(x => x.DeleteAsync(user));

            // Act
            IHttpActionResult userResponse = await _userController.DeleteUser(_testId);

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkResult;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteUserServiceMock.Verify(x => x.DeleteAsync(It.IsAny<User>()), Times.Once());
            _getUserServiceMock.Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetUser_HappyPath_ShouldGetUser()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            _getUserServiceMock.Setup(x => x.GetUserAsync(_testId.ToString())).ReturnsAsync(user);

            // Act
            IHttpActionResult userResponse = await _userController.GetUser(_testId);

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkNegotiatedContentResult<UserData>;
            var userResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<UserData>>(result, exactMatch: false);
            Assert.NotNull(userResult);

            _getUserServiceMock.Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetAllUsers_HappyPath_ShouldGetAllUsersMatchingFilters()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            var users = new List<User> { user };

            _getUserServiceMock.Setup(x => x.GetUsersAsync(null, userRequest.Name, null, null)).ReturnsAsync(users);

            // Act
            IHttpActionResult userResponse = await _userController.GetUsers(0, 1, null, userRequest.Name, null);

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkNegotiatedContentResult<List<UserData>>;
            var userResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<List<UserData>>>(result, exactMatch: false);
            Assert.Single(userResult);

            _getUserServiceMock.Verify(x => x.GetUsersAsync(It.IsAny<UserTypes>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteAllUsers_HappyPath_ShouldDeleteAllUsers()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            _deleteUserServiceMock.Setup(x => x.DeleteAllAsync());

            // Act
            IHttpActionResult userResponse = await _userController.DeleteAllUsers();

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkResult;
            var userResult = result;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteUserServiceMock.Verify(x => x.DeleteAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetUsersByTag_HappyPath_ShouldReturnUserByTag()
        {
            // Arrange            
            (UserModel userRequest, User user) = Arrange();

            var users = new List<User> { user };

            _getUserServiceMock.Setup(x => x.GetUsersAsync(null, null, null, "C")).ReturnsAsync(users);

            // Act
            IHttpActionResult userResponse = await _userController.GetUsersByTag("C");

            // Assert
            Assert.NotNull(userResponse);
            var result = userResponse as OkNegotiatedContentResult<IEnumerable<User>>;
            var userResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<IEnumerable<User>>>(result, exactMatch: false);
            Assert.Single(userResult);

            _getUserServiceMock.Verify(x => x.GetUserAsync(It.IsAny<string>()), Times.AtMost(2));
        }
    }
}
