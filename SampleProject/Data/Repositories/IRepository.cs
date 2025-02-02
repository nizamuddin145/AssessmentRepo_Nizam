using System;
using System.Threading.Tasks;
using BusinessEntities;

namespace Data.Repositories
{
    public interface IRepository<T> where T : IdObject
    {
        Task SaveAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetAsync(string id);
    }
}