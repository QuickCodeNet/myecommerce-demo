using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductAttributeValue;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductAttributeValue;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Api.Controllers
{
    public partial class ProductAttributeValuesController : QuickCodeBaseApiController
    {
        private readonly IProductAttributeValueService service;
        private readonly ILogger<ProductAttributeValuesController> logger;
        private readonly IServiceProvider serviceProvider;
        public ProductAttributeValuesController(IProductAttributeValueService service, IServiceProvider serviceProvider, ILogger<ProductAttributeValuesController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductAttributeValueDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "ProductAttributeValue", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "ProductAttributeValue") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductAttributeValueDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductAttributeValueDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(ProductAttributeValueDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "ProductAttributeValue") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, ProductAttributeValueDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-product-id/{productAttributeValueProductId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByProductIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByProductIdAsync(int productAttributeValueProductId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByProductIdAsync(productAttributeValueProductId, page, size);
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"ProductAttributeValueProductId: '{productAttributeValueProductId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-attributes-for-product/{productAttributeValuesProductId:int}/{productAttributeValuesAttributeId:int}/{productAttributesId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetAttributesForProductResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAttributesForProductAsync(int productAttributeValuesProductId, int productAttributeValuesAttributeId, int productAttributesId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetAttributesForProductAsync(productAttributeValuesProductId, productAttributeValuesAttributeId, productAttributesId, page, size);
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"ProductAttributeValuesProductId: '{productAttributeValuesProductId}', ProductAttributeValuesAttributeId: '{productAttributeValuesAttributeId}', ProductAttributesId: '{productAttributesId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpDelete("remove-by-product/{productAttributeValueProductId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> RemoveByProductAsync(int productAttributeValueProductId)
        {
            var response = await service.RemoveByProductAsync(productAttributeValueProductId);
            if (HandleResponseError(response, logger, "ProductAttributeValue", $"ProductAttributeValueProductId: '{productAttributeValueProductId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}