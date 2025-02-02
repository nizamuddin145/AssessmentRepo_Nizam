using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Shared.Models.Users;

namespace Core.Services.Users
{
    public interface ICreateUserService
    {
        Task<User> CreateAsync(Guid id, UserModel user);
    }
}