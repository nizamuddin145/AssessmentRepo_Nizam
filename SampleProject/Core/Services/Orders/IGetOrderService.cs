using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;

namespace Core.Services.Orders
{
    public interface IGetOrderService
    {
        Task<Order> GetOrderAsync(string id);

        Task<IEnumerable<Order>> GetOrdersAsync();
    }
}