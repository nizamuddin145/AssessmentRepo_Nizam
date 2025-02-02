using BusinessEntities;
using Common;
using Data.Repositories.Orders;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class DeleteOrderService : IDeleteOrderService
    {
        private readonly IOrderRepository _orderRepository;

        /// <summary>
        /// Constructor of the DeleteOrderService class.
        /// </summary>
        /// <param name="orderRepository">Order Repository for data storage.</param>
        public DeleteOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Deletes the Order.
        /// </summary>
        /// <param name="order">The Order object</param>
        public async Task DeleteAsync(Order order)
        {
            await _orderRepository.DeleteAsync(order);
        }

        /// <summary>
        /// Deletes all the orders.
        /// </summary>
        public async Task DeleteAllAsync()
        {
            await _orderRepository.DeleteAllAsync();
        }
    }
}