using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories.Users;
using Shared.Models.Users;

namespace Core.Services.Users
{
    [AutoRegister]
    public class UpdateUserService : IUpdateUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor of the UpdateUserService class.
        /// </summary>
        /// <param name="userRepository">User Repository.</param>
        public UpdateUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Updates the User.
        /// </summary>
        /// <param name="user">User object to be updated.</param>
        /// <param name="userModel">User model.</param>
        public async Task UpdateAsync(User user, UserModel userModel)
        {
            user.SetEmail(userModel.Email);
            user.SetName(userModel.Name);
            user.SetType(userModel.Type);
            user.SetAge(userModel.Age);
            user.SetMonthlySalary(userModel.AnnualSalary);
            user.SetTags(userModel.Tags);

            await _userRepository.SaveAsync(user);
        }
    }
}