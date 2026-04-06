using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Category;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Category;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Api.Controllers
{
    public partial class CategoriesController : QuickCodeBaseApiController
    {
        private readonly ICategoryService service;
        private readonly ILogger<CategoriesController> logger;
        private readonly IServiceProvider serviceProvider;
        public CategoriesController(ICategoryService service, IServiceProvider serviceProvider, ILogger<CategoriesController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CategoryDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Category", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Category") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Category", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CategoryDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(CategoryDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Category") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, CategoryDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Category", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Category", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-active/{categoryIsActive:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetActiveResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetActiveAsync(bool categoryIsActive, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetActiveAsync(categoryIsActive, page, size);
            if (HandleResponseError(response, logger, "Category", $"CategoryIsActive: '{categoryIsActive}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-slug/{categorySlug}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetBySlugResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetBySlugAsync(string categorySlug)
        {
            var response = await service.GetBySlugAsync(categorySlug);
            if (HandleResponseError(response, logger, "Category", $"CategorySlug: '{categorySlug}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-sub-categories/{categoryParentCategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetSubCategoriesResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetSubCategoriesAsync(int categoryParentCategoryId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetSubCategoriesAsync(categoryParentCategoryId, page, size);
            if (HandleResponseError(response, logger, "Category", $"CategoryParentCategoryId: '{categoryParentCategoryId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-root-categories/{categoryParentCategoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetRootCategoriesResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetRootCategoriesAsync(int categoryParentCategoryId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetRootCategoriesAsync(categoryParentCategoryId, page, size);
            if (HandleResponseError(response, logger, "Category", $"CategoryParentCategoryId: '{categoryParentCategoryId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("deactivate/{categoryId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeactivateAsync(int categoryId, [FromBody] DeactivateRequestDto updateRequest)
        {
            var response = await service.DeactivateAsync(categoryId, updateRequest);
            if (HandleResponseError(response, logger, "Category", $"CategoryId: '{categoryId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}