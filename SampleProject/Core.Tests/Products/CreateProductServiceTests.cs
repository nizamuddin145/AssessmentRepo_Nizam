using BusinessEntities;
using Core.Factories;
using Core.Services.Products;
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
    public class CreateProductServiceTests
    {
        private Mock<IIdObjectFactory<Product>> _factoryMock;
        private Mock<IUpdateProductService> _updateProductServiceMock;
        private Mock<IProductRepository> _productRepositoryMock;
        private Guid _testId = Guid.NewGuid();
        private CreateProductService _createProductService;

        public CreateProductServiceTests()
        {
            _factoryMock = new Mock<IIdObjectFactory<Product>>();
            _updateProductServiceMock = new Mock<IUpdateProductService>();
            _productRepositoryMock = new Mock<IProductRepository>();

            _createProductService = new CreateProductService(_factoryMock.Object, _productRepositoryMock.Object, _updateProductServiceMock.Object);
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
        public async Task CreateProduct_HappyPath_ShouldCreateAndReturnProduct()
        {
            // Arrange
            (ProductRequestModel productRequest, Product product) = Arrange();
            _factoryMock.Setup(x => x.Create(_testId)).Returns(product);
            _updateProductServiceMock.Setup(x => x.Update(product, productRequest));

            // Act
            Product productResult = await _createProductService.CreateAsync(_testId, productRequest);

            // Assert
            Assert.NotNull(productResult);
            Assert.IsType<Product>(productResult, exactMatch: false);
            Assert.Equal(productResult.ProductName, productRequest.Name);
            Assert.Equal(productResult.Description, productRequest.Description);
            Assert.Equal(productResult.Price, productRequest.Price);
            Assert.Equal(productResult.Quantity, productRequest.Quantity);

            _updateProductServiceMock.Verify(x => x.Update(It.IsAny<Product>(), It.IsAny<ProductRequestModel>()), Times.Once());
        }
    }
}
