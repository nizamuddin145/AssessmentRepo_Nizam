using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessEntities;
using Common;

namespace Data.Repositories.Orders
{
    [AutoRegister]
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        /// <summary>
        /// Get all the orders.
        /// </summary>
        /// <returns>List of all orders.</returns>
        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                return _memoryCache.Cast<DictionaryEntry>()
                .Select(entry => (Order)entry.Value)
                .ToList();
            });
        }

        /// <summary>
        /// Deletes all the orders.
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