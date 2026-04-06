using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.Refund;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.Refund;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Api.Controllers
{
    public partial class RefundsController : QuickCodeBaseApiController
    {
        private readonly IRefundService service;
        private readonly ILogger<RefundsController> logger;
        private readonly IServiceProvider serviceProvider;
        public RefundsController(IRefundService service, IServiceProvider serviceProvider, ILogger<RefundsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<RefundDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Refund", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Refund") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefundDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Refund", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(RefundDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(RefundDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Refund") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, RefundDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Refund", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Refund", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-payment-id/{refundPaymentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByPaymentIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByPaymentIdAsync(int refundPaymentId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByPaymentIdAsync(refundPaymentId, page, size);
            if (HandleResponseError(response, logger, "Refund", $"RefundPaymentId: '{refundPaymentId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-pending-refunds/{refundStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetPendingRefundsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPendingRefundsAsync(RefundStatus refundStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetPendingRefundsAsync(refundStatus, page, size);
            if (HandleResponseError(response, logger, "Refund", $"RefundStatus: '{refundStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("approve/{refundId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ApproveAsync(int refundId, [FromBody] ApproveRequestDto updateRequest)
        {
            var response = await service.ApproveAsync(refundId, updateRequest);
            if (HandleResponseError(response, logger, "Refund", $"RefundId: '{refundId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("process/{refundId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ProcessAsync(int refundId, [FromBody] ProcessRequestDto updateRequest)
        {
            var response = await service.ProcessAsync(refundId, updateRequest);
            if (HandleResponseError(response, logger, "Refund", $"RefundId: '{refundId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("reject/{refundId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> RejectAsync(int refundId, [FromBody] RejectRequestDto updateRequest)
        {
            var response = await service.RejectAsync(refundId, updateRequest);
            if (HandleResponseError(response, logger, "Refund", $"RefundId: '{refundId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}