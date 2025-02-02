using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;

namespace Data.Repositories.Users
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetDocumentAsync(UserTypes? userType = null, string name = null, string email = null, string tag = null);
        Task DeleteAllDcoumentAsync();
    }
}