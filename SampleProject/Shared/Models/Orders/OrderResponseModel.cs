using BusinessEntities;
using System;

namespace Shared.Models.Orders
{
    public class OrderResponseModel : IdObjectData
    {
        /// <summary>
        /// Order response model to initializes a new instance of a Order object.
        /// </summary>
        /// <param name="order">Order object.</param>
        public OrderResponseModel(Order order) : base(order)
        {
            ProductId = order.ProductId;
            Quantity = order.Quantity;
            OrderDateTime = order.OrderDateTime;
            TotalAmount = order.TotalAmount;
        }

        /// <summary>
        /// Product Id.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Product quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order Date and time.
        /// </summary>
        public DateTime OrderDateTime { get; set; }

        /// <summary>
        /// Total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}