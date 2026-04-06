using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.PaymentMethod;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.PaymentMethod;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Api.Controllers
{
    public partial class PaymentMethodsController : QuickCodeBaseApiController
    {
        private readonly IPaymentMethodService service;
        private readonly ILogger<PaymentMethodsController> logger;
        private readonly IServiceProvider serviceProvider;
        public PaymentMethodsController(IPaymentMethodService service, IServiceProvider serviceProvider, ILogger<PaymentMethodsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PaymentMethodDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "PaymentMethod", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "PaymentMethod") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaymentMethodDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "PaymentMethod", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PaymentMethodDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(PaymentMethodDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "PaymentMethod") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, PaymentMethodDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "PaymentMethod", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "PaymentMethod", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-customer-id/{paymentMethodCustomerId:int}/{paymentMethodIsActive:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByCustomerIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByCustomerIdAsync(int paymentMethodCustomerId, bool paymentMethodIsActive, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByCustomerIdAsync(paymentMethodCustomerId, paymentMethodIsActive, page, size);
            if (HandleResponseError(response, logger, "PaymentMethod", $"PaymentMethodCustomerId: '{paymentMethodCustomerId}', PaymentMethodIsActive: '{paymentMethodIsActive}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-default-method/{paymentMethodCustomerId:int}/{paymentMethodIsDefault:bool}/{paymentMethodIsActive:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDefaultMethodResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetDefaultMethodAsync(int paymentMethodCustomerId, bool paymentMethodIsDefault, bool paymentMethodIsActive)
        {
            var response = await service.GetDefaultMethodAsync(paymentMethodCustomerId, paymentMethodIsDefault, paymentMethodIsActive);
            if (HandleResponseError(response, logger, "PaymentMethod", $"PaymentMethodCustomerId: '{paymentMethodCustomerId}', PaymentMethodIsDefault: '{paymentMethodIsDefault}', PaymentMethodIsActive: '{paymentMethodIsActive}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("set-default/{paymentMethodId:int}/{paymentMethodCustomerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SetDefaultAsync(int paymentMethodId, int paymentMethodCustomerId, [FromBody] SetDefaultRequestDto updateRequest)
        {
            var response = await service.SetDefaultAsync(paymentMethodId, paymentMethodCustomerId, updateRequest);
            if (HandleResponseError(response, logger, "PaymentMethod", $"PaymentMethodId: '{paymentMethodId}', PaymentMethodCustomerId: '{paymentMethodCustomerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("deactivate/{paymentMethodId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeactivateAsync(int paymentMethodId, [FromBody] DeactivateRequestDto updateRequest)
        {
            var response = await service.DeactivateAsync(paymentMethodId, updateRequest);
            if (HandleResponseError(response, logger, "PaymentMethod", $"PaymentMethodId: '{paymentMethodId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}