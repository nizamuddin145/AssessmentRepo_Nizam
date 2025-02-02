using BusinessEntities;
using Core.Factories;
using Core.Services.Products;
using Data.Repositories.Products;
using Data.Repositories.Products;
using Moq;
using Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Tests.Products
{
    public class GetProductServiceTests
    {
        private Mock<IProductRepository> _productRepository;
        private Guid _testId = Guid.NewGuid();
        private GetProductService _getProductService;

        public GetProductServiceTests()
        {
            _productRepository = new Mock<IProductRepository>();

            _getProductService = new GetProductService(_productRepository.Object);
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
        public async Task GetProduct_HappyPath_ShouldGetProduct()
        {
            // Arrange
            (ProductRequestModel productRequest, Product product) = Arrange();
            _productRepository.Setup(x => x.GetAsync(_testId.ToString())).ReturnsAsync(product);

            // Act
            Product productResult = await _getProductService.GetProductAsync(_testId.ToString());

            // Assert
            Assert.NotNull(productResult);
            Assert.IsType<Product>(productResult, exactMatch: false);

            _productRepository.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task GetProducts_HappyPath_ShouldGetAllProducts()
        {
            // Arrange
            (ProductRequestModel productRequest, Product product) = Arrange();

            List<Product> products = new List<Product> { product };

            _productRepository.Setup(x => x.GetAllAsync(productRequest.Name, null)).ReturnsAsync(products);

            // Act
            IEnumerable<Product> productResult = await _getProductService.GetProductsAsync(productRequest.Name, null);

            // Assert
            Assert.NotNull(productResult);
            Assert.IsType<IEnumerable<Product>>(productResult, exactMatch: false);
            Assert.Single(productResult);

            _productRepository.Verify(x => x.GetAllAsync(It.IsAny<string>(), null), Times.Once());
        }
    }
}
