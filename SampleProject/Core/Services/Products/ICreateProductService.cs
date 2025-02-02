using System;
using System.Threading.Tasks;
using BusinessEntities;
using Shared.Models.Products;

namespace Core.Services.Products
{
    public interface ICreateProductService
    {
        Task<Product> CreateAsync(Guid id, ProductRequestModel requestModel);
    }
}