using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Repositories.Users;

namespace Core.Services.Users
{
    [AutoRegister]
    public class GetUserService : IGetUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor of Get User Service class.
        /// </summary>
        /// <param name="userRepository">User Repository.</param>
        public GetUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get User from store by Id.
        /// </summary>
        /// <param name="id">User Id.</param>
        /// <returns>The User object.</returns>
        public async Task<User> GetUserAsync(string id)
        {
            return await _userRepository.GetAsync(id);
        }

        /// <summary>
        /// Get the list of users based on the filters.
        /// </summary>
        /// <param name="userType">User type.</param>
        /// <param name="name">User name.</param>
        /// <param name="email">User email address.</param>
        /// <param name="tag">User tags.</param>
        /// <returns>A list of Users.</returns>
        public async Task<IEnumerable<User>> GetUsersAsync(UserTypes? userType = null, string name = null, string email = null, string tag = null)
        {
            return await _userRepository.GetDocumentAsync(userType, name, email, tag);
        }
    }
}