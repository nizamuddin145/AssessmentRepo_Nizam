using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Core.Factories;
using Shared.Models.Users;

namespace Core.Services.Users
{
    [AutoRegister]
    public class CreateUserService : ICreateUserService
    {
        private readonly IUpdateUserService _updateUserService;
        private readonly IIdObjectFactory<User> _userFactory;

        /// <summary>
        /// Constructor of CreateUserService class.
        /// </summary>
        /// <param name="userFactory">User Factory to create user object.</param>
        /// <param name="updateUserService">Service for updating the user details.</param>
        public CreateUserService(IIdObjectFactory<User> userFactory, IUpdateUserService updateUserService)
        {
            _userFactory = userFactory;
            _updateUserService = updateUserService;
        }

        /// <summary>
        /// Create a User with the details in store.
        /// </summary>
        /// /// <param name="id">User id.</param>
        /// <param name="userModel">User model.</param>
        /// <returns>User object.</returns>
        public async Task<User> CreateAsync(Guid id, UserModel userModel)
        {
            var user = _userFactory.Create(id);
            await _updateUserService.UpdateAsync(user, userModel);
            return user;
        }
    }
}