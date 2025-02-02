namespace BusinessEntities
{
    public class Product : IdObject
    {
        /// <summary>
        /// Product Name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Product Description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Product Price.
        /// </summary>
        public decimal? Price { get; set; }

        /// <summary>
        /// Product Quantity.
        /// </summary>
        public int Quantity { get; set; }
    }
}
