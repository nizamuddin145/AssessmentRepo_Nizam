using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using BusinessEntities;
using Common;

namespace Data.Repositories.Products
{
    [AutoRegister]
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {

        /// <summary>
        /// Get all the products with filters.
        /// </summary>
        /// <param name="name">Product name.</param>
        /// <param name="description">Product description.</param>
        /// <returns>List of products.</returns>
        public async Task<IEnumerable<Product>> GetAllAsync(string name = null, string description = null)
        {
            return await Task.Run(() =>
            {
                return _memoryCache.Cast<DictionaryEntry>()
                .Where(entry => entry.Value is Product product && (name == null || product.ProductName.Contains(name)) && (description == null || product.Description.Contains(description)))
                .Select(entry => (Product)entry.Value)
                .ToList();
            });
        }

        /// <summary>
        /// Deletes all the products.
        /// </summary>
        public async Task DeleteAllAsync()
        {
            await Task.Run(() =>
            {
                var caches = _memoryCache.ToList();

                foreach (var cache in caches)
                {
                    _memoryCache.Remove(cache.Key);
                }
            });
        }
    }
}