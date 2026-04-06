using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.Payment;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.Payment;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Api.Controllers
{
    public partial class PaymentsController : QuickCodeBaseApiController
    {
        private readonly IPaymentService service;
        private readonly ILogger<PaymentsController> logger;
        private readonly IServiceProvider serviceProvider;
        public PaymentsController(IPaymentService service, IServiceProvider serviceProvider, ILogger<PaymentsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Payment", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Payment") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Payment", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PaymentDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(PaymentDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Payment") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, PaymentDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Payment", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Payment", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-order-id/{paymentOrderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByOrderIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByOrderIdAsync(int paymentOrderId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByOrderIdAsync(paymentOrderId, page, size);
            if (HandleResponseError(response, logger, "Payment", $"PaymentOrderId: '{paymentOrderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-reference-id/{paymentReferenceId:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByReferenceIdResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByReferenceIdAsync(Guid paymentReferenceId)
        {
            var response = await service.GetByReferenceIdAsync(paymentReferenceId);
            if (HandleResponseError(response, logger, "Payment", $"PaymentReferenceId: '{paymentReferenceId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-status/{paymentStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByStatusResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByStatusAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByStatusAsync(paymentStatus, page, size);
            if (HandleResponseError(response, logger, "Payment", $"PaymentStatus: '{paymentStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-failed-payments/{paymentStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetFailedPaymentsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetFailedPaymentsAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetFailedPaymentsAsync(paymentStatus, page, size);
            if (HandleResponseError(response, logger, "Payment", $"PaymentStatus: '{paymentStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-payments-to-capture/{paymentStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetPaymentsToCaptureResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPaymentsToCaptureAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetPaymentsToCaptureAsync(paymentStatus, page, size);
            if (HandleResponseError(response, logger, "Payment", $"PaymentStatus: '{paymentStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-payment-with-gateway/{paymentsId:int}/{paymentsPaymentGatewayId:int}/{paymentGatewaysId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaymentWithGatewayResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetPaymentWithGatewayAsync(int paymentsId, int paymentsPaymentGatewayId, int paymentGatewaysId)
        {
            var response = await service.GetPaymentWithGatewayAsync(paymentsId, paymentsPaymentGatewayId, paymentGatewaysId);
            if (HandleResponseError(response, logger, "Payment", $"PaymentsId: '{paymentsId}', PaymentsPaymentGatewayId: '{paymentsPaymentGatewayId}', PaymentGatewaysId: '{paymentGatewaysId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-daily-volume/{paymentStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDailyVolumeResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetDailyVolumeAsync(PaymentStatus paymentStatus)
        {
            var response = await service.GetDailyVolumeAsync(paymentStatus);
            if (HandleResponseError(response, logger, "Payment", $"PaymentStatus: '{paymentStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("update-status/{paymentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateStatusAsync(int paymentId, [FromBody] UpdateStatusRequestDto updateRequest)
        {
            var response = await service.UpdateStatusAsync(paymentId, updateRequest);
            if (HandleResponseError(response, logger, "Payment", $"PaymentId: '{paymentId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-captured/{paymentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsCapturedAsync(int paymentId, [FromBody] MarkAsCapturedRequestDto updateRequest)
        {
            var response = await service.MarkAsCapturedAsync(paymentId, updateRequest);
            if (HandleResponseError(response, logger, "Payment", $"PaymentId: '{paymentId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-voided/{paymentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsVoidedAsync(int paymentId, [FromBody] MarkAsVoidedRequestDto updateRequest)
        {
            var response = await service.MarkAsVoidedAsync(paymentId, updateRequest);
            if (HandleResponseError(response, logger, "Payment", $"PaymentId: '{paymentId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}