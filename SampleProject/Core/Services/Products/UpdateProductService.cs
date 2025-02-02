using System.Collections.Generic;
using BusinessEntities;
using Common;
using Shared.Models.Products;

namespace Core.Services.Products
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateProductService : IUpdateProductService
    {
        /// <summary>
        /// Updates the Product object.
        /// </summary>
        /// <param name="product">The Product object to be updated.</param>
        /// <param name="requestModel">Product request model.</param>
        public void Update(Product product, ProductRequestModel requestModel)
        {
            product.ProductName = requestModel.Name.Trim();
            product.Description = requestModel.Description.Trim();
            product.Price = requestModel.Price;
            product.Quantity = requestModel.Quantity;
        }
    }
}