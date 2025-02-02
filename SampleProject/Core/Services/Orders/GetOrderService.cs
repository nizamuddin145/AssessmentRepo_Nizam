using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Data.Repositories.Orders;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor of the GetOrderService class.
        /// </summary>
        /// <param name="orderRepository">Repository for order data storage.</param>
        public GetOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// GEt the order by Id from store.
        /// </summary>
        /// <param name="id">The order Id.</param>
        /// <returns>The Order object.</returns>
        public async Task<Order> GetOrderAsync(string id)
        {
            return await _orderRepository.GetAsync(id);
        }

        /// <summary>
        /// Get all the Oder.
        /// </summary>
        /// <returns>A list of Order objects.</returns>
        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            return await _orderRepository.GetAllAsync();
        }
    }
}