using BusinessEntities;
using Core.Factories;
using Core.Services.Orders;
using Data.Repositories.Orders;
using Data.Repositories.Orders;
using Moq;
using Shared.Models.Orders;
using Shared.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.Orders
{
    public class GetOrderServiceTests
    {
        private Mock<IOrderRepository> _orderRepository;
        private Guid _testId = Guid.NewGuid();
        private GetOrderService _getOrderService;

        public GetOrderServiceTests()
        {
            _orderRepository = new Mock<IOrderRepository>();

            _getOrderService = new GetOrderService(_orderRepository.Object);
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
        public async Task GetOrder_HappyPath_ShouldGetOrder()
        {
            // Arrange
            (OrderRequestModel orderRequest, Order order) = Arrange();
            _orderRepository.Setup(x => x.GetAsync(_testId.ToString())).ReturnsAsync(order);

            // Act
            Order orderResult = await _getOrderService.GetOrderAsync(_testId.ToString());

            // Assert
            Assert.NotNull(orderResult);
            Assert.IsType<Order>(orderResult, exactMatch: false);

            _orderRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetOrders_HappyPath_ShouldGetAllOrders()
        {
            // Arrange
            (OrderRequestModel orderRequest, Order order) = Arrange();

            List<Order> orders = new List<Order> { order };

            _orderRepository.Setup(x => x.GetAllAsync()).ReturnsAsync(orders);

            // Act
            IEnumerable<Order> orderResult = await _getOrderService.GetOrdersAsync();

            // Assert
            Assert.NotNull(orderResult);
            Assert.IsType<IEnumerable<Order>>(orderResult, exactMatch: false);
            Assert.Single(orderResult);

            _orderRepository.Verify(x => x.GetAllAsync(), Times.Once());
        }
    }
}
