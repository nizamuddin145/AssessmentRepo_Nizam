using System;
using System.Collections.Generic;
using BusinessEntities;
using Common;
using Shared.Models.Orders;

namespace Core.Services.Orders
{
    [AutoRegister(AutoRegisterTypes.Singleton)]
    public class UpdateOrderService : IUpdateOrderService
    {
        /// <summary>
        /// Updates the Order object.
        /// </summary>
        /// <param name="order">The Order object.</param>
        /// <param name="requestModel">Order request model.</param>
        public void Update(Order order, OrderRequestModel requestModel)
        {
            order.ProductId = requestModel.ProductId;
            order.Quantity = requestModel.Quantity;
            order.OrderDateTime = requestModel.OrderDateTime;
            order.TotalAmount = requestModel.TotalAmount;
        }
    }
}