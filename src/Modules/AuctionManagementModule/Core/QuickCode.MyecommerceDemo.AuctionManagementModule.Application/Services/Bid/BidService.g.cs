using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Bid;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Bid
{
    public partial class BidService : IBidService
    {
        private readonly ILogger<BidService> _logger;
        private readonly IBidRepository _repository;
        public BidService(ILogger<BidService> logger, IBidRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<BidDto>> InsertAsync(BidDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(BidDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, BidDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<BidDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<BidDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByAuctionIdResponseDto>>> GetByAuctionIdAsync(int bidAuctionId, int? page, int? size)
        {
            var returnValue = await _repository.GetByAuctionIdAsync(bidAuctionId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetByBidderIdResponseDto>>> GetByBidderIdAsync(int bidBidderId, int? page, int? size)
        {
            var returnValue = await _repository.GetByBidderIdAsync(bidBidderId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetHighestBidResponseDto>> GetHighestBidAsync(int bidAuctionId)
        {
            var returnValue = await _repository.GetHighestBidAsync(bidAuctionId);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsOutbidAsync(int bidId, MarkAsOutbidRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsOutbidAsync(bidId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsWinningAsync(int bidId, MarkAsWinningRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsWinningAsync(bidId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}