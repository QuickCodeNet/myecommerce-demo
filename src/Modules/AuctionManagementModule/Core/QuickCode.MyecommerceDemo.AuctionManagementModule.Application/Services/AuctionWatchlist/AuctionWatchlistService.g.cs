using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionWatchlist;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionWatchlist
{
    public partial class AuctionWatchlistService : IAuctionWatchlistService
    {
        private readonly ILogger<AuctionWatchlistService> _logger;
        private readonly IAuctionWatchlistRepository _repository;
        public AuctionWatchlistService(ILogger<AuctionWatchlistService> logger, IAuctionWatchlistRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<AuctionWatchlistDto>> InsertAsync(AuctionWatchlistDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(AuctionWatchlistDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, AuctionWatchlistDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<AuctionWatchlistDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<AuctionWatchlistDto>> GetItemAsync(int id)
        {
            var returnValue = await _repository.GetByPkAsync(id);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteItemAsync(int id)
        {
            var deleteItem = await _repository.GetByPkAsync(id);
            if (deleteItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.DeleteAsync(deleteItem.Value);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> TotalItemCountAsync()
        {
            var returnValue = await _repository.CountAsync();
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetByUserIdResponseDto>>> GetByUserIdAsync(int auctionWatchlistUserId, int? page, int? size)
        {
            var returnValue = await _repository.GetByUserIdAsync(auctionWatchlistUserId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetWatchersByAuctionResponseDto>>> GetWatchersByAuctionAsync(int auctionWatchlistAuctionId, int? page, int? size)
        {
            var returnValue = await _repository.GetWatchersByAuctionAsync(auctionWatchlistAuctionId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> IsWatchingAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId)
        {
            var returnValue = await _repository.IsWatchingAsync(auctionWatchlistAuctionId, auctionWatchlistUserId);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> UnwatchAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId)
        {
            var returnValue = await _repository.UnwatchAsync(auctionWatchlistAuctionId, auctionWatchlistUserId);
            return returnValue.ToResponse();
        }
    }
}