using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Auction;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Auction;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Api.Controllers
{
    public partial class AuctionsController : QuickCodeBaseApiController
    {
        private readonly IAuctionService service;
        private readonly ILogger<AuctionsController> logger;
        private readonly IServiceProvider serviceProvider;
        public AuctionsController(IAuctionService service, IServiceProvider serviceProvider, ILogger<AuctionsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuctionDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "Auction", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "Auction") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "Auction", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuctionDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(AuctionDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "Auction") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, AuctionDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "Auction", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "Auction", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-active-auctions/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetActiveAuctionsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetActiveAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetActiveAuctionsAsync(auctionStatus, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-scheduled-auctions/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetScheduledAuctionsResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetScheduledAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetScheduledAuctionsAsync(auctionStatus, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-auctions-by-seller/{auctionSellerId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetAuctionsBySellerResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAuctionsBySellerAsync(int auctionSellerId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetAuctionsBySellerAsync(auctionSellerId, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionSellerId: '{auctionSellerId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-auctions-ending-soon/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetAuctionsEndingSoonResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAuctionsEndingSoonAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetAuctionsEndingSoonAsync(auctionStatus, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-auctions-to-close/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetAuctionsToCloseResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetAuctionsToCloseAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetAuctionsToCloseAsync(auctionStatus, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-closed-auctions-for-settlement/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetClosedAuctionsForSettlementResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetClosedAuctionsForSettlementAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetClosedAuctionsForSettlementAsync(auctionStatus, page, size);
            if (HandleResponseError(response, logger, "Auction", $"AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("start-auction/{auctionId:int}/{auctionStatus}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> StartAuctionAsync(int auctionId, AuctionStatus auctionStatus, [FromBody] StartAuctionRequestDto updateRequest)
        {
            var response = await service.StartAuctionAsync(auctionId, auctionStatus, updateRequest);
            if (HandleResponseError(response, logger, "Auction", $"AuctionId: '{auctionId}', AuctionStatus: '{auctionStatus}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("close-auction/{auctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CloseAuctionAsync(int auctionId, [FromBody] CloseAuctionRequestDto updateRequest)
        {
            var response = await service.CloseAuctionAsync(auctionId, updateRequest);
            if (HandleResponseError(response, logger, "Auction", $"AuctionId: '{auctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("cancel-auction/{auctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CancelAuctionAsync(int auctionId, [FromBody] CancelAuctionRequestDto updateRequest)
        {
            var response = await service.CancelAuctionAsync(auctionId, updateRequest);
            if (HandleResponseError(response, logger, "Auction", $"AuctionId: '{auctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPatch("set-winner/{auctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> SetWinnerAsync(int auctionId, [FromBody] SetWinnerRequestDto updateRequest)
        {
            var response = await service.SetWinnerAsync(auctionId, updateRequest);
            if (HandleResponseError(response, logger, "Auction", $"AuctionId: '{auctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}