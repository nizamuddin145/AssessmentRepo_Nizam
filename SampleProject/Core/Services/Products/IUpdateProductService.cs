using System.Collections.Generic;
using BusinessEntities;
using Shared.Models.Products;

namespace Core.Services.Products
{
    public interface IUpdateProductService
    {
        void Update(Product product, ProductRequestModel requestModel);
    }
}