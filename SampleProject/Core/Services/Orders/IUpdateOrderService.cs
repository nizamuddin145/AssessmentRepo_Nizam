using System;
using System.Collections.Generic;
using BusinessEntities;
using Shared.Models.Orders;

namespace Core.Services.Orders
{
    public interface IUpdateOrderService
    {
        void Update(Order order, OrderRequestModel requestModel);
    }
}