using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionSettlement;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionSettlement;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Api.Controllers
{
    public partial class AuctionSettlementsController : QuickCodeBaseApiController
    {
        private readonly IAuctionSettlementService service;
        private readonly ILogger<AuctionSettlementsController> logger;
        private readonly IServiceProvider serviceProvider;
        public AuctionSettlementsController(IAuctionSettlementService service, IServiceProvider serviceProvider, ILogger<AuctionSettlementsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuctionSettlementDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "AuctionSettlement", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "AuctionSettlement") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionSettlementDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "AuctionSettlement", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuctionSettlementDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(AuctionSettlementDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "AuctionSettlement") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, AuctionSettlementDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "AuctionSettlement", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "AuctionSettlement", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-auction-id/{auctionSettlementAuctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetByAuctionIdResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByAuctionIdAsync(int auctionSettlementAuctionId)
        {
            var response = await service.GetByAuctionIdAsync(auctionSettlementAuctionId);
            if (HandleResponseError(response, logger, "AuctionSettlement", $"AuctionSettlementAuctionId: '{auctionSettlementAuctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-outstanding-payments/{auctionSettlementIsPaid:bool}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetOutstandingPaymentsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetOutstandingPaymentsAsync(bool auctionSettlementIsPaid, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetOutstandingPaymentsAsync(auctionSettlementIsPaid, page, size);
            if (HandleResponseError(response, logger, "AuctionSettlement", $"AuctionSettlementIsPaid: '{auctionSettlementIsPaid}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-paid/{auctionSettlementId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsPaidAsync(int auctionSettlementId, [FromBody] MarkAsPaidRequestDto updateRequest)
        {
            var response = await service.MarkAsPaidAsync(auctionSettlementId, updateRequest);
            if (HandleResponseError(response, logger, "AuctionSettlement", $"AuctionSettlementId: '{auctionSettlementId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}