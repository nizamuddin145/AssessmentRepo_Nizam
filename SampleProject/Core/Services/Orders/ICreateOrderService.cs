using System;
using System.Threading.Tasks;
using BusinessEntities;
using Shared.Models.Orders;

namespace Core.Services.Orders
{
    public interface ICreateOrderService
    {
        Task<Order> CreateAsync(Guid id, OrderRequestModel requestModel);
    }
}