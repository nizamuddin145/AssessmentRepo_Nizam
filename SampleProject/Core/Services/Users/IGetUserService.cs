using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;

namespace Core.Services.Users
{
    public interface IGetUserService
    {
        Task<User> GetUserAsync(string id);

        Task<IEnumerable<User>> GetUsersAsync(UserTypes? userType = null, string name = null, string email = null, string tag = null);
    }
}