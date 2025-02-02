using BusinessEntities;
using Core.Services.Orders;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Models.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.Linq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests
{
    public class OrderControllerTests
    {
        private Mock<ILogger<OrderController>> _loggerMock;
        private Mock<ICreateOrderService> _createOrderServiceMock;
        private Mock<IDeleteOrderService> _deleteOrderServiceMock;
        private Mock<IGetOrderService> _getOrderServiceMock;
        private Mock<IUpdateOrderService> _updateOrderServiceMock;
        private Guid _testId = Guid.NewGuid();
        private OrderController _orderController;

        public OrderControllerTests()
        {
            _loggerMock = new Mock<ILogger<OrderController>>();
            _createOrderServiceMock = new Mock<ICreateOrderService>();
            _deleteOrderServiceMock = new Mock<IDeleteOrderService>();
            _getOrderServiceMock = new Mock<IGetOrderService>();
            _updateOrderServiceMock = new Mock<IUpdateOrderService>();

            _orderController = new OrderController(_loggerMock.Object, _createOrderServiceMock.Object, _deleteOrderServiceMock.Object, _getOrderServiceMock.Object, _updateOrderServiceMock.Object);
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
        public async Task CreateOrder_HappyPath_ShouldCreateAndReturnOrder()
        {
            // Arrange
            (OrderRequestModel orderRequest, Order order) = Arrange();
            _createOrderServiceMock.Setup(x => x.CreateAsync(_testId, orderRequest)).ReturnsAsync(order);

            // Act
            IHttpActionResult orderResponse = await _orderController.CreateOrder(_testId, orderRequest);

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as CreatedAtRouteNegotiatedContentResult<OrderResponseModel>;
            var orderResult = result.Content;
            Assert.IsType<CreatedAtRouteNegotiatedContentResult<OrderResponseModel>>(result, exactMatch: false);
            Assert.Equal(orderResult.ProductId,orderRequest.ProductId);
            Assert.Equal(orderResult.Quantity, orderRequest.Quantity);
            Assert.Equal(orderResult.OrderDateTime, orderRequest.OrderDateTime);
            Assert.Equal(orderResult.TotalAmount, orderRequest.TotalAmount);

            _createOrderServiceMock.Verify(x => x.CreateAsync(It.IsAny<Guid>(), It.IsAny<OrderRequestModel>()), Times.Once());
        }

        [Fact]
        public async Task UpdateOrder_HappyPath_ShouldUpdateAndReturnUpdatedOrder()
        {
            // Arrange            
            (OrderRequestModel orderRequest, Order order) = Arrange();

            _createOrderServiceMock.Setup(x => x.CreateAsync(_testId, orderRequest)).ReturnsAsync(order);
            _getOrderServiceMock.Setup(x => x.GetOrderAsync(_testId.ToString())).ReturnsAsync(order);
            orderRequest.Quantity = 5;
            _updateOrderServiceMock.Setup(x => x.Update(order, orderRequest));

            // Act
            IHttpActionResult orderResponse = await _orderController.UpdateOrder(_testId, orderRequest);

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as OkNegotiatedContentResult<OrderResponseModel>;
            var orderResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<OrderResponseModel>>(result, exactMatch: false);

            _updateOrderServiceMock.Verify(x => x.Update(It.IsAny<Order>(), It.IsAny<OrderRequestModel>()), Times.Once());
            _getOrderServiceMock.Verify(x => x.GetOrderAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteOrder_HappyPath_ShouldDeleteOrder()
        {
            // Arrange            
            (OrderRequestModel orderRequest, Order order) = Arrange();

            _getOrderServiceMock.Setup(x => x.GetOrderAsync(_testId.ToString())).ReturnsAsync(order);
            _deleteOrderServiceMock.Setup(x => x.DeleteAsync(order));

            // Act
            IHttpActionResult orderResponse = await _orderController.DeleteOrder(_testId);

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as OkResult;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteOrderServiceMock.Verify(x => x.DeleteAsync(It.IsAny<Order>()), Times.Once());
            _getOrderServiceMock.Verify(x => x.GetOrderAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetOrder_HappyPath_ShouldGetOrder()
        {
            // Arrange            
            (OrderRequestModel orderRequest, Order order) = Arrange();

            _getOrderServiceMock.Setup(x => x.GetOrderAsync(_testId.ToString())).ReturnsAsync(order);

            // Act
            IHttpActionResult orderResponse = await _orderController.GetOrder(_testId);

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as OkNegotiatedContentResult<OrderResponseModel>;
            var orderResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<OrderResponseModel>>(result, exactMatch: false);
            Assert.NotNull(orderResult);

            _getOrderServiceMock.Verify(x => x.GetOrderAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetAllOrders_HappyPath_ShouldGetAllOrdersMatchingFilters()
        {
            // Arrange            
            (OrderRequestModel orderRequest, Order order) = Arrange();

            var orders = new List<Order> { order };

            _getOrderServiceMock.Setup(x => x.GetOrdersAsync()).ReturnsAsync(orders);

            // Act
            IHttpActionResult orderResponse = await _orderController.GetOrders();

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as OkNegotiatedContentResult<IEnumerable<Order>>;
            var orderResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<IEnumerable<Order>>>(result, exactMatch: false);
            Assert.Single(orderResult);

            _getOrderServiceMock.Verify(x => x.GetOrdersAsync(), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteAllOrders_HappyPath_ShouldDeleteAllOrders()
        {
            // Arrange            
            (OrderRequestModel orderRequest, Order order) = Arrange();

            _deleteOrderServiceMock.Setup(x => x.DeleteAllAsync());

            // Act
            IHttpActionResult orderResponse = await _orderController.DeleteAllOrders();

            // Assert
            Assert.NotNull(orderResponse);
            var result = orderResponse as OkResult;
            var OrderResult = result;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteOrderServiceMock.Verify(x => x.DeleteAllAsync(), Times.Once);
        }
    }
}
