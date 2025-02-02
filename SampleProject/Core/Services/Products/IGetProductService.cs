using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;

namespace Core.Services.Products
{
    public interface IGetProductService
    {
        Task<Product> GetProductAsync(string id);

        Task<IEnumerable<Product>> GetProductsAsync(string name = null, string description = null);
    }
}