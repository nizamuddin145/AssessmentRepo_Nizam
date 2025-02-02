using BusinessEntities;
using Common;
using Data.Repositories.Users;
using System.Threading.Tasks;

namespace Core.Services.Users
{
    [AutoRegister]
    public class DeleteUserService : IDeleteUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor of DeleteUserService class..
        /// </summary>
        /// <param name="userRepository">User Repository for storeage.</param>
        public DeleteUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Delete the user from the store.
        /// </summary>
        /// <param name="id">The User Id.</param>
        public async Task DeleteAsync(User user)
        {
            await _userRepository.DeleteAsync(user);
        }

        /// <summary>
        /// Deletes all the users from the store.
        /// </summary>
        public async Task DeleteAllAsync()
        {
            await _userRepository.DeleteAllDcoumentAsync();
        }
    }
}