using BusinessEntities;
using Core.Factories;
using Core.Services.Orders;
using Data.Repositories.Orders;
using Data.Repositories.Products;
using Moq;
using Shared.Models.Orders;
using Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.Orders
{
    public class CreateOrderServiceTests
    {
        private Mock<IIdObjectFactory<Order>> _factoryMock;
        private Mock<IUpdateOrderService> _updateOrderServiceMock;
        private Mock<IOrderRepository> _orderRepositoryMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Guid _testId = Guid.NewGuid();
        private CreateOrderService _createOrderService;

        public CreateOrderServiceTests()
        {
            _factoryMock = new Mock<IIdObjectFactory<Order>>();
            _updateOrderServiceMock = new Mock<IUpdateOrderService>();
            _orderRepositoryMock = new Mock<IOrderRepository>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _createOrderService = new CreateOrderService(_factoryMock.Object, _orderRepositoryMock.Object, _updateOrderServiceMock.Object, _productRepositoryMock.Object);
        }

        public (OrderRequestModel orderRequest, Order Order, Product product) Arrange()
        {
            var orderRequest = new OrderRequestModel
            {
                ProductId = _testId,
                Quantity = 2,
                OrderDateTime = DateTime.Now,
                TotalAmount = 4,
            };

            var order = new Order();
            order.ProductId = orderRequest.ProductId;
            order.Quantity = orderRequest.Quantity;
            order.OrderDateTime = orderRequest.OrderDateTime;
            order.TotalAmount = orderRequest.TotalAmount;

            var product = new Product
            {
                ProductName = "Laptop",
                Description = "Manufactured by Apple",
                Price = 200,
                Quantity = 1,
            };

            return (orderRequest, order, product);
        }

        [Fact]
        public async Task CreateOrder_HappyPath_ShouldCreateAndReturnOrder()
        {
            // Arrange
            (OrderRequestModel orderRequest, Order order, Product product) = Arrange();
            _factoryMock.Setup(x => x.Create(_testId)).Returns(order);
            _updateOrderServiceMock.Setup(x => x.Update(order, orderRequest));
            _productRepositoryMock.Setup(x => x.GetAsync(_testId.ToString())).ReturnsAsync(product);

            // Act
            Order orderResult = await _createOrderService.CreateAsync(_testId, orderRequest);

            // Assert
            Assert.NotNull(orderResult);
            Assert.IsType<Order>(orderResult, exactMatch: false);
            Assert.Equal(orderResult.ProductId, orderRequest.ProductId);
            Assert.Equal(orderResult.Quantity, orderRequest.Quantity);
            Assert.Equal(orderResult.OrderDateTime, orderRequest.OrderDateTime);
            Assert.Equal(orderResult.TotalAmount, orderRequest.TotalAmount);

            _updateOrderServiceMock.Verify(x => x.Update(It.IsAny<Order>(), It.IsAny<OrderRequestModel>()), Times.Once());
            _productRepositoryMock.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once());
        }
    }
}
