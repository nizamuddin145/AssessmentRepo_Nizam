using System;

namespace BusinessEntities
{
    public class Order : IdObject
    {
        /// <summary>
        /// Prdouct Id of the order.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Order Quantity.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Order Date and time.
        /// </summary>
        public DateTime OrderDateTime { get; set; }

        /// <summary>
        /// Order Total amount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}
