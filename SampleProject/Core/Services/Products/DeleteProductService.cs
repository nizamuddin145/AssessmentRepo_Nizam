using BusinessEntities;
using Common;
using Data.Repositories.Products;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor of the DeleteProductService class.
        /// </summary>
        /// <param name="productRepository">Repository for product data storage.</param>
        public DeleteProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        /// <summary>
        /// Delete the Product.
        /// </summary>
        /// <param name="product">The Product object.</param>
        public async Task DeleteAsync(Product product)
        {
            await _productRepository.DeleteAsync(product);
        }

        /// <summary>
        /// Deletes all products.
        /// </summary>
        public async Task DeleteAllAsync()
        {
            await _productRepository.DeleteAllAsync();
        }
    }
}