using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Core.Services.Products;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Shared.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly ICreateProductService _createProductService;
        private readonly IDeleteProductService _deleteProductService;
        private readonly IGetProductService _getProductService;
        private readonly IUpdateProductService _updateProductService;

        public ProductController(ILogger<ProductController> logger, ICreateProductService createProductService, IDeleteProductService deleteProductService, IGetProductService getProductService, IUpdateProductService updateProductService)
        {
            _logger = logger;
            _createProductService = createProductService;
            _deleteProductService = deleteProductService;
            _getProductService = getProductService;
            _updateProductService = updateProductService;
        }

        [HttpPost]
        [Route("{productId:guid}/create", Name = "CreateProduct")]
        public async Task<IHttpActionResult> CreateProduct(Guid productId, [FromBody] ProductRequestModel productRequestModel)
        {
            _logger.LogInformation($"Creating the Product for productId: {productId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingProduct = await _getProductService.GetProductAsync(productId.ToString());

            if (existingProduct != null)
            {
                _logger.LogWarning($"Product for Id {productId} already exists.");
                return Ok(new ProductResponseModel(existingProduct));
            }
            var product = await _createProductService.CreateAsync(productId, productRequestModel);
            _logger.LogInformation($"Product created successfully for Product Id: {productId}");

            return CreatedAtRoute("CreateProduct", new { id = product.Id }, new ProductResponseModel(product));
        }

        [HttpGet]
        [Route("{productId:guid}", Name = "GetProduct")]
        public async Task<IHttpActionResult> GetProduct(Guid productId)
        {
            _logger.LogInformation($"Getting the Product for productId: {productId}");

            var product = await _getProductService.GetProductAsync(productId.ToString());

            if (product == null)
            {
                _logger.LogWarning($"Product for Id {productId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Product for Id {productId} retrieved successfully.");
            return Ok(new ProductResponseModel(product));
        }

        [HttpGet]
        [Route("list", Name = "GetProducts")]
        public async Task<IHttpActionResult> GetProducts(string name, string description = null)
        {
            _logger.LogInformation($"Getting the Products for filters: {name}, description: {description}");

            var products = await _getProductService.GetProductsAsync(name, description);

            if (!products.Any())
            {
                _logger.LogInformation($"No products found with the specified criteria.");
                return NotFound();
            }

            _logger.LogInformation($"Retrieved {products.Count()} products.");

            return Ok(products);
        }

        [HttpPut]
        [Route("{productId:guid}/update", Name = "UpdateProduct")]
        public async Task<IHttpActionResult> UpdateProduct(Guid productId, [FromBody] ProductRequestModel productRequestModel)
        {
            _logger.LogInformation($"Updating the Product for productId: {productId}");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _getProductService.GetProductAsync(productId.ToString());

            if (product == null)
            {
                _logger.LogWarning($"Product for Id {productId} not found.");
                return NotFound();
            }

            _updateProductService.Update(product, productRequestModel);
            _logger.LogInformation($"Product for Id {productId} updated successfully.");

            return Ok(new ProductResponseModel(product));
        }

        [HttpDelete]
        [Route("{productId:guid}/delete", Name = "DeleteProduct")]
        public async Task<IHttpActionResult> DeleteProduct(Guid productId)
        {
            _logger.LogInformation($"Deleting the for productId: {productId}");

            var product = await _getProductService.GetProductAsync(productId.ToString());
            if (product == null)
            {
                _logger.LogWarning($"Product for Id {productId} not found.");

                return NotFound();
            }

            await _deleteProductService.DeleteAsync(product);
            _logger.LogInformation($"Product for Id {productId} deleted successfully.");

            return Ok();
        }

        [Route("clear", Name = "DeleteProducts")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAllProducts()
        {
            _logger.LogInformation("Deleting all the Products.");

            await _deleteProductService.DeleteAllAsync();
            _logger.LogInformation("All products deleted successfully.");

            return Ok();
        }
    }
}