using BusinessEntities;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    public interface IDeleteOrderService
    {
        Task DeleteAsync(Order order);
        Task DeleteAllAsync();
    }
}