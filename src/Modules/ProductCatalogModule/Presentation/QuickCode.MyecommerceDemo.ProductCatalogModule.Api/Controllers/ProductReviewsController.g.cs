using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductReview;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductReview;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Api.Controllers
{
    public partial class ProductReviewsController : QuickCodeBaseApiController
    {
        private readonly IProductReviewService service;
        private readonly ILogger<ProductReviewsController> logger;
        private readonly IServiceProvider serviceProvider;
        public ProductReviewsController(IProductReviewService service, IServiceProvider serviceProvider, ILogger<ProductReviewsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ProductReviewDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "ProductReview", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "ProductReview") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductReviewDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "ProductReview", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ProductReviewDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(ProductReviewDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "ProductReview") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, ProductReviewDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "ProductReview", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "ProductReview", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-product-id/{productReviewProductId:int}/{productReviewIsApproved:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByProductIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByProductIdAsync(int productReviewProductId, bool productReviewIsApproved, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByProductIdAsync(productReviewProductId, productReviewIsApproved, page, size);
            if (HandleResponseError(response, logger, "ProductReview", $"ProductReviewProductId: '{productReviewProductId}', ProductReviewIsApproved: '{productReviewIsApproved}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-pending-approval/{productReviewIsApproved:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetPendingApprovalResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPendingApprovalAsync(bool productReviewIsApproved, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetPendingApprovalAsync(productReviewIsApproved, page, size);
            if (HandleResponseError(response, logger, "ProductReview", $"ProductReviewIsApproved: '{productReviewIsApproved}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-customer-reviews/{productReviewCustomerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetCustomerReviewsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetCustomerReviewsAsync(int productReviewCustomerId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetCustomerReviewsAsync(productReviewCustomerId, page, size);
            if (HandleResponseError(response, logger, "ProductReview", $"ProductReviewCustomerId: '{productReviewCustomerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-average-rating/{productReviewProductId:int}/{productReviewIsApproved:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAverageRatingResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAverageRatingAsync(int productReviewProductId, bool productReviewIsApproved)
        {
            var response = await service.GetAverageRatingAsync(productReviewProductId, productReviewIsApproved);
            if (HandleResponseError(response, logger, "ProductReview", $"ProductReviewProductId: '{productReviewProductId}', ProductReviewIsApproved: '{productReviewIsApproved}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("approve/{productReviewId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApproveAsync(int productReviewId, [FromBody] ApproveRequestDto updateRequest)
        {
            var response = await service.ApproveAsync(productReviewId, updateRequest);
            if (HandleResponseError(response, logger, "ProductReview", $"ProductReviewId: '{productReviewId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}