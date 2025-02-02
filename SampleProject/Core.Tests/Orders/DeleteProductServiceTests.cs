using BusinessEntities;
using Core.Factories;
using Core.Services.Orders;
using Data.Repositories.Orders;
using Moq;
using Shared.Models.Orders;
using Shared.Models.Orders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Core.Tests.Orders
{
    public class DeleteOrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepository;
        private Guid _testId = Guid.NewGuid();
        private DeleteOrderService _deleteOrderService;

        public DeleteOrderServiceTests()
        {
            _orderRepository = new Mock<IOrderRepository>();

            _deleteOrderService = new DeleteOrderService(_orderRepository.Object);
        }

        public (OrderRequestModel orderRequest, Order Order) Arrange()
        {
            var orderRequest = new OrderRequestModel
            {
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                OrderDateTime = DateTime.Now,
                TotalAmount = 4,
            };

            var order = new Order();
            order.ProductId = orderRequest.ProductId;
            order.Quantity = orderRequest.Quantity;
            order.OrderDateTime = orderRequest.OrderDateTime;
            order.TotalAmount = orderRequest.TotalAmount;

            return (orderRequest, order);
        }

        [Fact]
        public async Task DeleteOrder_HappyPath_ShouldDeleteOrder()
        {
            // Arrange
            (OrderRequestModel orderRequest, Order order) = Arrange();
            _orderRepository.Setup(x => x.DeleteAsync(order));

            // Act
            await _deleteOrderService.DeleteAsync(order);

            // Assert
            _orderRepository.Verify(x => x.DeleteAsync(It.IsAny<Order>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAllOrder_HappyPath_ShouldDeleteOrder()
        {
            // Arrange
            _orderRepository.Setup(x => x.DeleteAllAsync());

            // Act
            await _deleteOrderService.DeleteAllAsync();

            // Assert
            _orderRepository.Verify(x => x.DeleteAllAsync(), Times.Once());
        }
    }
}
