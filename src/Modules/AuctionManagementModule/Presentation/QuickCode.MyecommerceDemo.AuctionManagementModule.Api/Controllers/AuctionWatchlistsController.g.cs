using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using QuickCode.MyecommerceDemo.Common.Controllers;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionWatchlist;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionWatchlist;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Api.Controllers
{
    public partial class AuctionWatchlistsController : QuickCodeBaseApiController
    {
        private readonly IAuctionWatchlistService service;
        private readonly ILogger<AuctionWatchlistsController> logger;
        private readonly IServiceProvider serviceProvider;
        public AuctionWatchlistsController(IAuctionWatchlistService service, IServiceProvider serviceProvider, ILogger<AuctionWatchlistsController> logger)
        {
            this.service = service;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AuctionWatchlistDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> ListAsync([FromQuery] int? page, int? size)
        {
            if (ValidatePagination(page, size) is {} error)
                return error;
            var response = await service.ListAsync(page, size);
            if (HandleResponseError(response, logger, "AuctionWatchlist", "List") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> CountAsync()
        {
            var response = await service.TotalItemCountAsync();
            if (HandleResponseError(response, logger, "AuctionWatchlist") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuctionWatchlistDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetItemAsync(int id)
        {
            var response = await service.GetItemAsync(id);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuctionWatchlistDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> InsertAsync(AuctionWatchlistDto model)
        {
            var response = await service.InsertAsync(model);
            if (HandleResponseError(response, logger, "AuctionWatchlist") is {} responseError)
                return responseError;
            return CreatedAtRoute(new { id = response.Value.Id }, response.Value);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UpdateAsync(int id, AuctionWatchlistDto model)
        {
            if (!(model.Id == id))
            {
                return BadRequest($"Id: '{id}' must be equal to model.Id: '{model.Id}'");
            }

            var response = await service.UpdateAsync(id, model);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"Id: '{id}'") is {} responseError)
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
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"Id: '{id}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-by-user-id/{auctionWatchlistUserId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetByUserIdResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetByUserIdAsync(int auctionWatchlistUserId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetByUserIdAsync(auctionWatchlistUserId, page, size);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"AuctionWatchlistUserId: '{auctionWatchlistUserId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("get-watchers-by-auction/{auctionWatchlistAuctionId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<GetWatchersByAuctionResponseDto>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> GetWatchersByAuctionAsync(int auctionWatchlistAuctionId, int? page, int? size)
        {
            if (page < 1)
            {
                var pageNumberError = $"Page Number must be greater than 1";
                logger.LogWarning($"List Error: '{pageNumberError}''");
                return NotFound(pageNumberError);
            }

            var response = await service.GetWatchersByAuctionAsync(auctionWatchlistAuctionId, page, size);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"AuctionWatchlistAuctionId: '{auctionWatchlistAuctionId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpGet("is-watching/{auctionWatchlistAuctionId:int}/{auctionWatchlistUserId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> IsWatchingAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId)
        {
            var response = await service.IsWatchingAsync(auctionWatchlistAuctionId, auctionWatchlistUserId);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"AuctionWatchlistAuctionId: '{auctionWatchlistAuctionId}', AuctionWatchlistUserId: '{auctionWatchlistUserId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }

        [HttpDelete("unwatch/{auctionWatchlistAuctionId:int}/{auctionWatchlistUserId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        public async Task<IActionResult> UnwatchAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId)
        {
            var response = await service.UnwatchAsync(auctionWatchlistAuctionId, auctionWatchlistUserId);
            if (HandleResponseError(response, logger, "AuctionWatchlist", $"AuctionWatchlistAuctionId: '{auctionWatchlistAuctionId}', AuctionWatchlistUserId: '{auctionWatchlistUserId}'") is {} responseError)
                return responseError;
            return Ok(response.Value);
        }
    }
}