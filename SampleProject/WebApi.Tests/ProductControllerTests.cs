using BusinessEntities;
using Core.Services.Products;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.Linq;
using WebApi.Controllers;
using Xunit;

namespace WebApi.Tests
{
    public class ProductControllerTests
    {
        private Mock<ILogger<ProductController>> _loggerMock;
        private Mock<ICreateProductService> _createProductServiceMock;
        private Mock<IDeleteProductService> _deleteProductServiceMock;
        private Mock<IGetProductService> _getProductServiceMock;
        private Mock<IUpdateProductService> _updateProductServiceMock;
        private Guid _testId = Guid.NewGuid();
        private ProductController _productController;

        public ProductControllerTests()
        {
            _loggerMock = new Mock<ILogger<ProductController>>();
            _createProductServiceMock = new Mock<ICreateProductService>();
            _deleteProductServiceMock = new Mock<IDeleteProductService>();
            _getProductServiceMock = new Mock<IGetProductService>();
            _updateProductServiceMock = new Mock<IUpdateProductService>();

            _productController = new ProductController(_loggerMock.Object, _createProductServiceMock.Object, _deleteProductServiceMock.Object, _getProductServiceMock.Object, _updateProductServiceMock.Object);
        }

        public (ProductRequestModel productRequest, Product Product) Arrange()
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
            _createProductServiceMock.Setup(x => x.CreateAsync(_testId, productRequest)).ReturnsAsync(product);

            // Act
            IHttpActionResult productResponse = await _productController.CreateProduct(_testId, productRequest);

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as CreatedAtRouteNegotiatedContentResult<ProductResponseModel>;
            var productResult = result.Content;
            Assert.IsType<CreatedAtRouteNegotiatedContentResult<ProductResponseModel>>(result, exactMatch: false);
            Assert.Equal(productResult.Name,productRequest.Name);
            Assert.Equal(productResult.Description, productRequest.Description);
            Assert.Equal(productResult.Price, productRequest.Price);
            Assert.Equal(productResult.Quantity, productRequest.Quantity);

            _createProductServiceMock.Verify(x => x.CreateAsync(It.IsAny<Guid>(), It.IsAny<ProductRequestModel>()), Times.Once());
        }

        [Fact]
        public async Task UpdateProduct_HappyPath_ShouldUpdateAndReturnUpdatedProduct()
        {
            // Arrange            
            (ProductRequestModel productRequest, Product product) = Arrange();

            _createProductServiceMock.Setup(x => x.CreateAsync(_testId, productRequest)).ReturnsAsync(product);
            _getProductServiceMock.Setup(x => x.GetProductAsync(_testId.ToString())).ReturnsAsync(product);
            productRequest.Name = "Test";
            _updateProductServiceMock.Setup(x => x.Update(product, productRequest));

            // Act
            IHttpActionResult productResponse = await _productController.UpdateProduct(_testId, productRequest);

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as OkNegotiatedContentResult<ProductResponseModel>;
            var productResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<ProductResponseModel>>(result, exactMatch: false);

            _updateProductServiceMock.Verify(x => x.Update(It.IsAny<Product>(), It.IsAny<ProductRequestModel>()), Times.Once());
            _getProductServiceMock.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteProduct_HappyPath_ShouldDeleteProduct()
        {
            // Arrange            
            (ProductRequestModel productRequest, Product product) = Arrange();

            _getProductServiceMock.Setup(x => x.GetProductAsync(_testId.ToString())).ReturnsAsync(product);
            _deleteProductServiceMock.Setup(x => x.DeleteAsync(product));

            // Act
            IHttpActionResult productResponse = await _productController.DeleteProduct(_testId);

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as OkResult;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteProductServiceMock.Verify(x => x.DeleteAsync(It.IsAny<Product>()), Times.Once());
            _getProductServiceMock.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetProduct_HappyPath_ShouldGetProduct()
        {
            // Arrange            
            (ProductRequestModel productRequest, Product product) = Arrange();

            _getProductServiceMock.Setup(x => x.GetProductAsync(_testId.ToString())).ReturnsAsync(product);

            // Act
            IHttpActionResult productResponse = await _productController.GetProduct(_testId);

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as OkNegotiatedContentResult<ProductResponseModel>;
            var productResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<ProductResponseModel>>(result, exactMatch: false);
            Assert.NotNull(productResult);

            _getProductServiceMock.Verify(x => x.GetProductAsync(It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task GetAllProducts_HappyPath_ShouldGetAllProductsMatchingFilters()
        {
            // Arrange            
            (ProductRequestModel productRequest, Product product) = Arrange();

            var products = new List<Product> { product };

            _getProductServiceMock.Setup(x => x.GetProductsAsync(productRequest.Name, null)).ReturnsAsync(products);

            // Act
            IHttpActionResult productResponse = await _productController.GetProducts(productRequest.Name, null);

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as OkNegotiatedContentResult<IEnumerable<Product>>;
            var productResult = result.Content;
            Assert.IsType<OkNegotiatedContentResult<IEnumerable<Product>>>(result, exactMatch: false);
            Assert.Single(productResult);

            _getProductServiceMock.Verify(x => x.GetProductsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.AtMost(2));
        }

        [Fact]
        public async Task DeleteAllProducts_HappyPath_ShouldDeleteAllProducts()
        {
            // Arrange            
            (ProductRequestModel productRequest, Product product) = Arrange();

            _deleteProductServiceMock.Setup(x => x.DeleteAllAsync());

            // Act
            IHttpActionResult productResponse = await _productController.DeleteAllProducts();

            // Assert
            Assert.NotNull(productResponse);
            var result = productResponse as OkResult;
            var ProductResult = result;
            Assert.IsType<OkResult>(result, exactMatch: false);

            _deleteProductServiceMock.Verify(x => x.DeleteAllAsync(), Times.Once);
        }
    }
}
