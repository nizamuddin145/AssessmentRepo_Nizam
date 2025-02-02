using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Repositories.Products;

namespace Core.Services.Products
{
    [AutoRegister]
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor of the GetProductService class.
        /// </summary>
        /// <param name="productRepository">Repository for product data storage.</param>
        public GetProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Get the product by Product Id.
        /// </summary>
        /// <param name="id">The product Id.</param>
        /// <returns>The Product object.</returns>
        public async Task<Product> GetProductAsync(string id)
        {
            return await _productRepository.GetAsync(id);
        }

        /// <summary>
        /// List of Products based on the filter criteria.
        /// </summary>
        /// <param name="name">Product name.</param>
        /// <param name="description">Product description.</param>
        /// <returns>Products list.</returns>
        public async Task<IEnumerable<Product>> GetProductsAsync(string name = null, string description = null)
        {
            return await _productRepository.GetAllAsync(name, description);
        }
    }
}