using Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace Data.Repositories
{
    [AutoRegister]
    public class InMemoryRepository<T> : IRepository<T> where T : IdObject
    {
        protected readonly MemoryCache _memoryCache = MemoryCache.Default;
        private CacheItemPolicy policy = new CacheItemPolicy()
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(15)
        };

        /// <summary>
        /// Delete the entity.
        /// </summary>
        /// <param name="entiry">An entity to be deleted.</param>
        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _memoryCache.Remove(entity.Id));
        }

        /// <summary>
        /// Get the entity from Memory cache.
        /// </summary>
        /// <param name="id">The Id of the entiry.</param>
        /// <returns>An entity.</returns>
        public async Task<T> GetAsync(string id)
        {            
            return await Task.FromResult(_memoryCache.Get(id) as T);
        }

        /// <summary>
        /// Saves the entity in Memory cache.
        /// </summary>
        /// <param name="entity">The entiry to be saved.</param>
        public async Task SaveAsync(T entity)
        {
            await Task.Run(() =>
            {
                _memoryCache.Set(entity.Id, entity, policy);
            });            
        }
    }
}
