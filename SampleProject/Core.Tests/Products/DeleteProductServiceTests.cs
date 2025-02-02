using BusinessEntities;
using Core.Factories;
using Core.Services.Products;
using Data.Repositories.Products;
using Moq;
using Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xunit;

namespace Core.Tests.Products
{
    public class DeleteProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Guid _testId = Guid.NewGuid();
        private DeleteProductService _deleteProductService;

        public DeleteProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();

            _deleteProductService = new DeleteProductService(_productRepository.Object);
        }

        public (ProductRequestModel productRequest, Product product) Arrange()
        {
            var productRequest = new ProductRequestModel
            {
                Name = "Laptop",
                Description = "Manufactured by Apple",
                Price = 200,
                Quantity = 1,
            };

            var product = new Product();
            product.ProductName = productRequest.Name;
            product.Description = productRequest.Description;
            product.Price = productRequest.Price;
            product.Quantity = productRequest.Quantity;

            return (productRequest, product);
        }

        [Fact]
        public async Task DeleteProduct_HappyPath_ShouldDeleteProduct()
        {
            // Arrange
            (ProductRequestModel productRequest, Product product) = Arrange();
            _productRepository.Setup(x => x.DeleteAsync(product));

            // Act
            await _deleteProductService.DeleteAsync(product);

            // Assert
            _productRepository.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Once());
        }

        [Fact]
        public async Task DeleteAllProduct_HappyPath_ShouldDeleteProduct()
        {
            // Arrange
            _productRepository.Setup(x => x.DeleteAllAsync());

            // Act
            await _deleteProductService.DeleteAllAsync();

            // Assert
            _productRepository.Verify(x => x.DeleteAllAsync(), Times.Once());
        }
    }
}
