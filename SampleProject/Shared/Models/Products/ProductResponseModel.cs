using BusinessEntities;

namespace Shared.Models.Products
{
    public class ProductResponseModel : IdObjectData
    {
        /// <summary>
        /// Product response model to initializes a new instance of a Product object.
        /// </summary>
        /// <param name="product">Product object.</param>
        public ProductResponseModel(Product product) : base(product)
        {
            Name = product.ProductName;
            Description = product.Description;
            Price = product.Price;
            Quantity = product.Quantity;
        }

        /// <summary>
        /// Product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Product description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Product quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}