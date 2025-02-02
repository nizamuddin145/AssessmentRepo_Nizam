using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Orders
{
    public class OrderRequestModel
    {
        /// <summary>
        /// Product Id, a required field.
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// Product quantity, a required field and must be between 0 and max value.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        /// <summary>
        /// Order date and time, a required field.
        /// </summary>
        [Required]
        public DateTime OrderDateTime { get; set; }

        /// <summary>
        /// Total amount of the order, a required and must be greater than 0 and less than max value.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}