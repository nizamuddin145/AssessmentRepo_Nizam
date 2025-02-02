using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Products;
using Data.Repositories.Products;
using Shared.Models.Products;
using Shared.Validation;

namespace Core.Services.Products
{
    [AutoRegister]
    public class CreateProductService : ICreateProductService
    {
        private readonly IUpdateProductService _updateProductService;
        private readonly IIdObjectFactory<Product> _productFactory;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Constructor of the CreateProductService class.
        /// </summary>
        /// <param name="productFactory">Product factory.</param>
        /// <param name="productRepository">Product Repository for the storage.</param>
        /// <param name="updateProductService">Service for updating the product details.</param>
        public CreateProductService(IIdObjectFactory<Product> productFactory, IProductRepository productRepository, IUpdateProductService updateProductService)
        {
            _productFactory = productFactory;
            _productRepository = productRepository;
            _updateProductService = updateProductService;
        }

        /// <summary>
        /// Creates a Product in storage.
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <param name="requestModel">Product request model.</param>
        /// <returns>The Product object.</returns>
        public async Task<Product> CreateAsync(Guid id, ProductRequestModel requestModel)
        {
            var product = _productFactory.Create(id);
            _updateProductService.Update(product, requestModel);
            await _productRepository.SaveAsync(product);
            return product;
        }
    }
}