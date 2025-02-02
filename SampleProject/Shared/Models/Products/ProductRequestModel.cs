using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Products
{
    public class ProductRequestModel
    {
        /// <summary>
        /// Product name, a required field and maximum of 255 characters.
        /// </summary>
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product price, a required field and between 0 and max value.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public decimal? Price { get; set; }

        /// <summary>
        /// Product quantity, a required field and between 0 and max value.
        /// </summary>
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}