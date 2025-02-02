using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Orders;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Models.Orders;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly ICreateOrderService _createOrderService;
        private readonly IDeleteOrderService _deleteOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateOrderService _updateOrderService;

        public OrderController(ILogger<OrderController> logger, ICreateOrderService createOrderService, IDeleteOrderService deleteOrderService, IGetOrderService getOrderService, IUpdateOrderService updateOrderService)
        {
            _logger = logger;
            _createOrderService = createOrderService;
            _deleteOrderService = deleteOrderService;
            _getOrderService = getOrderService;
            _updateOrderService = updateOrderService;
        }

        [HttpPost]
        [Route("{orderId:guid}/create", Name = "CreateOrder")]
        public async Task<IHttpActionResult> CreateOrder(Guid orderId, [FromBody] OrderRequestModel orderRequestModel)
        {
            _logger.LogInformation($"Creating the Order for orderId: {orderId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingOrder = await _getOrderService.GetOrderAsync(orderId.ToString());

            if (existingOrder != null)
            {
                _logger.LogWarning($"Order for Id {orderId} already exists.");
                return Ok(new OrderResponseModel(existingOrder));
            }

            var order = await _createOrderService.CreateAsync(orderId, orderRequestModel);
            _logger.LogInformation($"Order for Id {orderId} created successfully.");

            return CreatedAtRoute("CreateOrder", new { id = order.Id }, new OrderResponseModel(order));
        }

        [HttpGet]
        [Route("{orderId:guid}", Name = "GetOrder")]
        public async Task<IHttpActionResult> GetOrder(Guid orderId)
        {
            _logger.LogInformation($"Getting the Order for orderId: {orderId}");

            var order = await _getOrderService.GetOrderAsync(orderId.ToString());

            if (order == null)
            {
                _logger.LogWarning($"Order for Id {orderId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Order for Id {orderId} retrieved successfully.");
            return Ok(new OrderResponseModel(order));
        }

        [HttpGet]
        [Route("list", Name = "GetOrders")]
        public async Task<IHttpActionResult> GetOrders()
        {
            _logger.LogInformation("Getting the Orders.");

            var orders = await _getOrderService.GetOrdersAsync();

            if (!orders.Any())
            {
                _logger.LogInformation($"No orders found.");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved {orders.Count()} orders.");
            return Ok(orders);
        }

        [HttpPut]
        [Route("{orderId:guid}/update", Name = "UpdateOrder")]
        public async Task<IHttpActionResult> UpdateOrder(Guid orderId, [FromBody] OrderRequestModel orderRequestModel)
        {
            _logger.LogInformation($"Updating the Order for orderId: {orderId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _getOrderService.GetOrderAsync(orderId.ToString());

            if (order == null)
            {
                _logger.LogWarning($"Order for Id {orderId} not found.");
                return NotFound();
            }

            _updateOrderService.Update(order, orderRequestModel);
            _logger.LogInformation($"Order for Id {orderId} updated successfully.");

            return Ok(new OrderResponseModel(order));
        }

        [HttpDelete]
        [Route("{orderId:guid}/delete", Name = "DeleteOrder")]
        public async Task<IHttpActionResult> DeleteOrder(Guid orderId)
        {
            _logger.LogInformation($"Deleting the Order for orderId: {orderId}");

            var order = await _getOrderService.GetOrderAsync(orderId.ToString());
            if (order == null)
            {
                _logger.LogWarning($"Order for Id {orderId} not found.");
                return NotFound();
            }

            await _deleteOrderService.DeleteAsync(order);
            _logger.LogInformation($"Order for Id {orderId} deleted successfully.");

            return Ok();
        }

        [Route("clear", Name = "DeleteOrders")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAllOrders()
        {
            _logger.LogInformation("Deleting all the Oders.");

            await _deleteOrderService.DeleteAllAsync();
            _logger.LogInformation("All orders deleted successfully.");

            return Ok();
        }
    }
}