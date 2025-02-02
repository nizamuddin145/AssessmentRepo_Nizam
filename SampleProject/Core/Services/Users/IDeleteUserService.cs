using BusinessEntities;
using System.Threading.Tasks;

namespace Core.Services.Users
{
    public interface IDeleteUserService
    {
        Task DeleteAsync(User user);
        Task DeleteAllAsync();
    }
}