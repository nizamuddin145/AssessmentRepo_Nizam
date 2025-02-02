using BusinessEntities;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    public interface IDeleteProductService
    {
        Task DeleteAsync(Product product);
        Task DeleteAllAsync();
    }
}