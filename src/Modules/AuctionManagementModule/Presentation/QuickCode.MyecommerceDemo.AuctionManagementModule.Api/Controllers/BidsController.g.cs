using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Bid;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Bid;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Api.Controllers
{
    public partial class BidsController : QuickCodeBaseApiController
    {
        private readonly IBidService service;
        private readonly ILogger<BidsController> logger;
        private readonly IServiceProvider serviceProvider;
        public BidsController(IBidService service, IServiceProvider serviceProvider, ILogger<BidsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<BidDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Bid", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Bid") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BidDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Bid", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(BidDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(BidDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Bid") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, BidDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Bid", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Bid", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-auction-id/{bidAuctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByAuctionIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByAuctionIdAsync(int bidAuctionId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByAuctionIdAsync(bidAuctionId, page, size);
            if (HandleResponseError(response, logger, "Bid", $"BidAuctionId: '{bidAuctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-bidder-id/{bidBidderId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByBidderIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByBidderIdAsync(int bidBidderId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByBidderIdAsync(bidBidderId, page, size);
            if (HandleResponseError(response, logger, "Bid", $"BidBidderId: '{bidBidderId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-highest-bid/{bidAuctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetHighestBidResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetHighestBidAsync(int bidAuctionId)
        {
            var response = await service.GetHighestBidAsync(bidAuctionId);
            if (HandleResponseError(response, logger, "Bid", $"BidAuctionId: '{bidAuctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-outbid/{bidId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsOutbidAsync(int bidId, [FromBody] MarkAsOutbidRequestDto updateRequest)
        {
            var response = await service.MarkAsOutbidAsync(bidId, updateRequest);
            if (HandleResponseError(response, logger, "Bid", $"BidId: '{bidId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("mark-as-winning/{bidId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> MarkAsWinningAsync(int bidId, [FromBody] MarkAsWinningRequestDto updateRequest)
        {
            var response = await service.MarkAsWinningAsync(bidId, updateRequest);
            if (HandleResponseError(response, logger, "Bid", $"BidId: '{bidId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}