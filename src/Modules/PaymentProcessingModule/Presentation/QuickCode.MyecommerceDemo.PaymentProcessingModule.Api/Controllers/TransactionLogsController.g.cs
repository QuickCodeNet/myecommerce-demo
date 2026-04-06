using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.TransactionLog;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.TransactionLog;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Api.Controllers
{
    public partial class TransactionLogsController : QuickCodeBaseApiController
    {
        private readonly ITransactionLogService service;
        private readonly ILogger<TransactionLogsController> logger;
        private readonly IServiceProvider serviceProvider;
        public TransactionLogsController(ITransactionLogService service, IServiceProvider serviceProvider, ILogger<TransactionLogsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TransactionLogDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "TransactionLog", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "TransactionLog") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TransactionLogDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(long id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "TransactionLog", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TransactionLogDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(TransactionLogDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "TransactionLog") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(long id, TransactionLogDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "TransactionLog", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var response = await service.DeleteItemAsync(id);
            if (HandleResponseError(response, logger, "TransactionLog", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-payment-id/{transactionLogPaymentId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByPaymentIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByPaymentIdAsync(int transactionLogPaymentId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByPaymentIdAsync(transactionLogPaymentId, page, size);
            if (HandleResponseError(response, logger, "TransactionLog", $"TransactionLogPaymentId: '{transactionLogPaymentId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("search-logs/{transactionLogMessage}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SearchLogsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SearchLogsAsync(string transactionLogMessage, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.SearchLogsAsync(transactionLogMessage, page, size);
            if (HandleResponseError(response, logger, "TransactionLog", $"TransactionLogMessage: '{transactionLogMessage}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}