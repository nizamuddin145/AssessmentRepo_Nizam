using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Orders;
using Data.Repositories.Orders;
using Data.Repositories.Products;
using Shared.Models.Orders;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor of the CreateOrderService class.
        /// </summary>
        /// <param name="updateOrderService">Order update service.</param>
        /// <param name="orderFactory">Order Factory.</param>
        /// <param name="orderRepository">Order Repository.</param>
        /// <param name="productRepository">Product Repository.</param>
        public CreateOrderService(
            IIdObjectFactory<Order> orderFactory, 
            IOrderRepository orderRepository, 
            IUpdateOrderService updateOrderService,
            IProductRepository productRepository)
        {
            _updateOrderService = updateOrderService;
            _orderFactory = orderFactory;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Creates an Order with details.
        /// </summary>
        /// <param name="id">Order Id.</param>
        /// <param name="requestModel">Order Request model.</param>
        /// <returns>The Order object.</returns>
        public async Task<Order> CreateAsync(Guid id, OrderRequestModel requestModel)
        {
            var order = _orderFactory.Create(id);

            var product = await _productRepository.GetAsync(requestModel.ProductId.ToString());

            if (product == null)
            {
                throw new Exception($"Product not found for Order Id {id}");
            }

            _updateOrderService.Update(order, requestModel);
            await _orderRepository.SaveAsync(order);
            return order;
        }
    }
}