using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Routing;
using BusinessEntities;
using Core.Services.Users;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ILogger<UserController> _logger;
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        /// <summary>
        /// Constructor to initialize the user controller.
        /// </summary>
        /// <param name="logger">Logger to log the information.</param>
        /// <param name="createUserService">Service to create the user.</param>
        /// <param name="deleteUserService">Service to delete the user.</param>
        /// <param name="getUserService">Service to get the user.</param>
        /// <param name="updateUserService">Service to update the user.</param>
        public UserController(ILogger<UserController> logger, ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _logger = logger;
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        /// <summary>
        /// Create the user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userModel">User model</param>
        /// <returns>User data with Status OK</returns>
        [Route("{userId:guid}/create", Name = "CreateUser")]
        [HttpPost]
        public async Task<IHttpActionResult> CreateUser(Guid userId, [FromBody] UserModel userModel)
        {
            _logger.LogInformation($"Creating the User for userId: {userId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _getUserService.GetUserAsync(userId.ToString());
            if (existingUser != null)
            {
                _logger.LogWarning($"User for Id {userId} is already exists.");
                return Ok(new UserData(existingUser));
            }

            var user = await _createUserService.CreateAsync(userId, userModel);
            _logger.LogInformation($"User created successfully for Id {userId}.");

            return CreatedAtRoute("CreateUser", new { id = user.Id }, new UserData(user));
        }

        /// <summary>
        /// Update the user
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="userModel">User model</param>
        /// <returns>User data with Status OK</returns>
        [Route("{userId:guid}/update", Name = "UpdateUser")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUser(Guid userId, [FromBody] UserModel userModel)
        {
            _logger.LogInformation($"Updating the User for userId: {userId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _getUserService.GetUserAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning($"User for id {userId} not found.");
                return NotFound();
            }

            await _updateUserService.UpdateAsync(user, userModel);
            _logger.LogInformation($"User for id {userId} updated successfully.");

            return Ok(new UserData(user));
        }

        /// <summary>
        /// Delete the user.
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Status OK</returns>
        [Route("{userId:guid}/delete", Name = "DeleteUser")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteUser(Guid userId)
        {
            _logger.LogInformation($"Deleting the User for userId: {userId}");

            var user = await _getUserService.GetUserAsync(userId.ToString());
            if (user == null)
            {
                _logger.LogWarning($"User for id {userId} not found.");
                return NotFound();
            }

            await _deleteUserService.DeleteAsync(user);

            _logger.LogInformation($"User for ID {userId} deleted successfully.");

            return Ok();
        }

        /// <summary>
        /// Get the user details
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>User data with Status OK</returns>
        [Route("{userId:guid}", Name = "GetUser")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUser(Guid userId)
        {
            _logger.LogInformation($"Getting the User for userId: {userId}");

            var user = await _getUserService.GetUserAsync(userId.ToString());

            if (user == null)
            {
                _logger.LogWarning($"User for id {userId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"User for id {userId} retrieved successfully.");
            return Ok(new UserData(user));
        }

        /// <summary>
        /// Get all users based with a search criteria.
        /// </summary>
        /// <param name="skip">Number to bypasses a specified number of elements in a sequence and then returns the remaining elements.</param>
        /// <param name="take">Specified number of contiguous elements from the start of a sequence.</param>
        /// <param name="type">User Type.</param>
        /// <param name="name">User Name.</param>
        /// <returns>List of Users.</returns>
        [Route("list", Name = "GetAllUsers")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            _logger.LogInformation($"Getting the Users for filters skip: {skip}, take: {take}, type: {type}, name: {name}, email: {email}");

            var users = (await _getUserService.GetUsersAsync(type, name, email))
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            if (!users.Any())
            {
                _logger.LogInformation($"No users found for the specified criteria.");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved the {users.Count} users.");
            return Ok(users);
        }

        /// <summary>
        /// Delete all the users.
        /// </summary>
        /// <returns>Ok Status</returns>
        [Route("clear", Name = "DeleteAllUsers")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAllUsers()
        {
            _logger.LogInformation("Deleting all the users.");

            await _deleteUserService.DeleteAllAsync();
            _logger.LogInformation("All users deleted successfully.");

            return Ok();
        }

        /// <summary>
        /// Get the users by tag.
        /// </summary>
        /// <param name="tag">Tag identifier</param>
        /// <returns>List of users matching for the tag and OK status</returns>

        [Route("list/tag")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUsersByTag(string tag)
        {
            _logger.LogInformation($"Get the Users By tag {tag}");

            var users = await _getUserService.GetUsersAsync(tag: tag);

            if (!users.Any())
            {
                _logger.LogInformation($"No users found for tag: {tag}.");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved the {users.Count()} users for tag {tag}.");
            return Ok(users);
        }
    }
}