using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Product;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Product;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Api.Controllers
{
    public partial class ProductsController : QuickCodeBaseApiController
    {
        private readonly IProductService service;
        private readonly ILogger<ProductsController> logger;
        private readonly IServiceProvider serviceProvider;
        public ProductsController(IProductService service, IServiceProvider serviceProvider, ILogger<ProductsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Product", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Product") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Product", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(ProductDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Product") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, ProductDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Product", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var response = await service.DeleteItemAsync(id);
            if (HandleResponseError(response, logger, "Product", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-active/{productStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetActiveResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetActiveAsync(ProductStatus productStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetActiveAsync(productStatus, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductStatus: '{productStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-featured/{productStatus}/{productIsFeatured:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetFeaturedResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetFeaturedAsync(ProductStatus productStatus, bool productIsFeatured, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetFeaturedAsync(productStatus, productIsFeatured, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductStatus: '{productStatus}', ProductIsFeatured: '{productIsFeatured}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("search-by-name/{productName}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SearchByNameResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SearchByNameAsync(string productName, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.SearchByNameAsync(productName, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductName: '{productName}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-sku/{productSku}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBySkuResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetBySkuAsync(string productSku)
        {
            var response = await service.GetBySkuAsync(productSku);
            if (HandleResponseError(response, logger, "Product", $"ProductSku: '{productSku}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-category/{productCategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByCategoryResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByCategoryAsync(int productCategoryId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByCategoryAsync(productCategoryId, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductCategoryId: '{productCategoryId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-low-stock/{productStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetLowStockResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetLowStockAsync(ProductStatus productStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetLowStockAsync(productStatus, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductStatus: '{productStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-products-with-details/{productsCategoryId:int}/{productBrandId:int}/{categoryId:int}/{brandId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetProductsWithDetailsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetProductsWithDetailsAsync(int productsCategoryId, int productBrandId, int categoryId, int brandId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetProductsWithDetailsAsync(productsCategoryId, productBrandId, categoryId, brandId, page, size);
            if (HandleResponseError(response, logger, "Product", $"ProductsCategoryId: '{productsCategoryId}', ProductBrandId: '{productBrandId}', CategoryId: '{categoryId}', BrandId: '{brandId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-recently-added")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetRecentlyAddedResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetRecentlyAddedAsync(DateTime productCreatedDate, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetRecentlyAddedAsync(productCreatedDate, page, size);
            if (HandleResponseError(response, logger, "Product", $"") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("update-price/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdatePriceAsync(int productId, [FromBody] UpdatePriceRequestDto updateRequest)
        {
            var response = await service.UpdatePriceAsync(productId, updateRequest);
            if (HandleResponseError(response, logger, "Product", $"ProductId: '{productId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("adjust-stock/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> AdjustStockAsync(int productId, [FromBody] AdjustStockRequestDto updateRequest)
        {
            var response = await service.AdjustStockAsync(productId, updateRequest);
            if (HandleResponseError(response, logger, "Product", $"ProductId: '{productId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("set-status/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SetStatusAsync(int productId, [FromBody] SetStatusRequestDto updateRequest)
        {
            var response = await service.SetStatusAsync(productId, updateRequest);
            if (HandleResponseError(response, logger, "Product", $"ProductId: '{productId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("archive/{productId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ArchiveAsync(int productId, [FromBody] ArchiveRequestDto updateRequest)
        {
            var response = await service.ArchiveAsync(productId, updateRequest);
            if (HandleResponseError(response, logger, "Product", $"ProductId: '{productId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}