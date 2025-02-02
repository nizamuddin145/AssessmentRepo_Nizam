using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Shared.Models.Users;

namespace Core.Services.Users
{
    public interface IUpdateUserService
    {
        Task UpdateAsync(User user, UserModel userModel);
    }
}