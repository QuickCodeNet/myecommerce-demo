using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.Order;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.Order;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Api.Controllers
{
    public partial class OrdersController : QuickCodeBaseApiController
    {
        private readonly IOrderService service;
        private readonly ILogger<OrdersController> logger;
        private readonly IServiceProvider serviceProvider;
        public OrdersController(IOrderService service, IServiceProvider serviceProvider, ILogger<OrdersController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrderDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Order", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Order") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Order", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrderDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(OrderDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Order") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, OrderDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Order", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Order", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-customer-id/{orderCustomerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByCustomerIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByCustomerIdAsync(int orderCustomerId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByCustomerIdAsync(orderCustomerId, page, size);
            if (HandleResponseError(response, logger, "Order", $"OrderCustomerId: '{orderCustomerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-order-number/{orderOrderNumber}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByOrderNumberResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByOrderNumberAsync(string orderOrderNumber)
        {
            var response = await service.GetByOrderNumberAsync(orderOrderNumber);
            if (HandleResponseError(response, logger, "Order", $"OrderOrderNumber: '{orderOrderNumber}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-status/{orderStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByStatusResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByStatusAsync(OrderStatus orderStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByStatusAsync(orderStatus, page, size);
            if (HandleResponseError(response, logger, "Order", $"OrderStatus: '{orderStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-recent-orders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetRecentOrdersResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetRecentOrdersAsync(DateTime orderCreatedDate, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetRecentOrdersAsync(orderCreatedDate, page, size);
            if (HandleResponseError(response, logger, "Order", $"") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-orders-for-fulfillment/{orderStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetOrdersForFulfillmentResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetOrdersForFulfillmentAsync(OrderStatus orderStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetOrdersForFulfillmentAsync(orderStatus, page, size);
            if (HandleResponseError(response, logger, "Order", $"OrderStatus: '{orderStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-order-with-details/{ordersId:int}/{orderShippingAddressId:int}/{orderBillingAddressId:int}/{addressId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderWithDetailsResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetOrderWithDetailsAsync(int ordersId, int orderShippingAddressId, int orderBillingAddressId, int addressId)
        {
            var response = await service.GetOrderWithDetailsAsync(ordersId, orderShippingAddressId, orderBillingAddressId, addressId);
            if (HandleResponseError(response, logger, "Order", $"OrdersId: '{ordersId}', OrderShippingAddressId: '{orderShippingAddressId}', OrderBillingAddressId: '{orderBillingAddressId}', AddressId: '{addressId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-monthly-revenue")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetMonthlyRevenueResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetMonthlyRevenueAsync(DateTime orderCreatedDate)
        {
            var response = await service.GetMonthlyRevenueAsync(orderCreatedDate);
            if (HandleResponseError(response, logger, "Order", $"") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("update-status/{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateStatusAsync(int orderId, [FromBody] UpdateStatusRequestDto updateRequest)
        {
            var response = await service.UpdateStatusAsync(orderId, updateRequest);
            if (HandleResponseError(response, logger, "Order", $"OrderId: '{orderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-paid/{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsPaidAsync(int orderId, [FromBody] MarkAsPaidRequestDto updateRequest)
        {
            var response = await service.MarkAsPaidAsync(orderId, updateRequest);
            if (HandleResponseError(response, logger, "Order", $"OrderId: '{orderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-shipped/{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsShippedAsync(int orderId, [FromBody] MarkAsShippedRequestDto updateRequest)
        {
            var response = await service.MarkAsShippedAsync(orderId, updateRequest);
            if (HandleResponseError(response, logger, "Order", $"OrderId: '{orderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("cancel-order/{orderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CancelOrderAsync(int orderId, [FromBody] CancelOrderRequestDto updateRequest)
        {
            var response = await service.CancelOrderAsync(orderId, updateRequest);
            if (HandleResponseError(response, logger, "Order", $"OrderId: '{orderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}