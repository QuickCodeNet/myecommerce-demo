using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.Auction;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.Auction
{
    public partial class AuctionService : IAuctionService
    {
        private readonly ILogger<AuctionService> _logger;
        private readonly IAuctionRepository _repository;
        public AuctionService(ILogger<AuctionService> logger, IAuctionRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<AuctionDto>> InsertAsync(AuctionDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(AuctionDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, AuctionDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<AuctionDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<AuctionDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveAuctionsResponseDto>>> GetActiveAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAuctionsAsync(auctionStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetScheduledAuctionsResponseDto>>> GetScheduledAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetScheduledAuctionsAsync(auctionStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetAuctionsBySellerResponseDto>>> GetAuctionsBySellerAsync(int auctionSellerId, int? page, int? size)
        {
            var returnValue = await _repository.GetAuctionsBySellerAsync(auctionSellerId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetAuctionsEndingSoonResponseDto>>> GetAuctionsEndingSoonAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetAuctionsEndingSoonAsync(auctionStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetAuctionsToCloseResponseDto>>> GetAuctionsToCloseAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetAuctionsToCloseAsync(auctionStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetClosedAuctionsForSettlementResponseDto>>> GetClosedAuctionsForSettlementAsync(AuctionStatus auctionStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetClosedAuctionsForSettlementAsync(auctionStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> StartAuctionAsync(int auctionId, AuctionStatus auctionStatus, StartAuctionRequestDto updateRequest)
        {
            var returnValue = await _repository.StartAuctionAsync(auctionId, auctionStatus, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> CloseAuctionAsync(int auctionId, CloseAuctionRequestDto updateRequest)
        {
            var returnValue = await _repository.CloseAuctionAsync(auctionId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> CancelAuctionAsync(int auctionId, CancelAuctionRequestDto updateRequest)
        {
            var returnValue = await _repository.CancelAuctionAsync(auctionId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> SetWinnerAsync(int auctionId, SetWinnerRequestDto updateRequest)
        {
            var returnValue = await _repository.SetWinnerAsync(auctionId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}