using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.Address;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.Address;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Api.Controllers
{
    public partial class AddressesController : QuickCodeBaseApiController
    {
        private readonly IAddressService service;
        private readonly ILogger<AddressesController> logger;
        private readonly IServiceProvider serviceProvider;
        public AddressesController(IAddressService service, IServiceProvider serviceProvider, ILogger<AddressesController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddressDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Address", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Address") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddressDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Address", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddressDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(AddressDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Address") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, AddressDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Address", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Address", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-customer-id/{addressCustomerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByCustomerIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByCustomerIdAsync(int addressCustomerId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByCustomerIdAsync(addressCustomerId, page, size);
            if (HandleResponseError(response, logger, "Address", $"AddressCustomerId: '{addressCustomerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-default-shipping/{addressCustomerId:int}/{addressIsDefaultShipping:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDefaultShippingResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetDefaultShippingAsync(int addressCustomerId, bool addressIsDefaultShipping)
        {
            var response = await service.GetDefaultShippingAsync(addressCustomerId, addressIsDefaultShipping);
            if (HandleResponseError(response, logger, "Address", $"AddressCustomerId: '{addressCustomerId}', AddressIsDefaultShipping: '{addressIsDefaultShipping}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-default-billing/{addressCustomerId:int}/{addressIsDefaultBilling:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetDefaultBillingResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetDefaultBillingAsync(int addressCustomerId, bool addressIsDefaultBilling)
        {
            var response = await service.GetDefaultBillingAsync(addressCustomerId, addressIsDefaultBilling);
            if (HandleResponseError(response, logger, "Address", $"AddressCustomerId: '{addressCustomerId}', AddressIsDefaultBilling: '{addressIsDefaultBilling}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("set-default-shipping/{addressId:int}/{addressCustomerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SetDefaultShippingAsync(int addressId, int addressCustomerId, [FromBody] SetDefaultShippingRequestDto updateRequest)
        {
            var response = await service.SetDefaultShippingAsync(addressId, addressCustomerId, updateRequest);
            if (HandleResponseError(response, logger, "Address", $"AddressId: '{addressId}', AddressCustomerId: '{addressCustomerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}