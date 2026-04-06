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
    public partial interface IAuctionService
    {
        Task<Response<AuctionDto>> InsertAsync(AuctionDto request);
        Task<Response<bool>> DeleteAsync(AuctionDto request);
        Task<Response<bool>> UpdateAsync(int id, AuctionDto request);
        Task<Response<List<AuctionDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<AuctionDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveAuctionsResponseDto>>> GetActiveAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size);
        Task<Response<List<GetScheduledAuctionsResponseDto>>> GetScheduledAuctionsAsync(AuctionStatus auctionStatus, int? page, int? size);
        Task<Response<List<GetAuctionsBySellerResponseDto>>> GetAuctionsBySellerAsync(int auctionSellerId, int? page, int? size);
        Task<Response<List<GetAuctionsEndingSoonResponseDto>>> GetAuctionsEndingSoonAsync(AuctionStatus auctionStatus, int? page, int? size);
        Task<Response<List<GetAuctionsToCloseResponseDto>>> GetAuctionsToCloseAsync(AuctionStatus auctionStatus, int? page, int? size);
        Task<Response<List<GetClosedAuctionsForSettlementResponseDto>>> GetClosedAuctionsForSettlementAsync(AuctionStatus auctionStatus, int? page, int? size);
        Task<Response<int>> StartAuctionAsync(int auctionId, AuctionStatus auctionStatus, StartAuctionRequestDto updateRequest);
        Task<Response<int>> CloseAuctionAsync(int auctionId, CloseAuctionRequestDto updateRequest);
        Task<Response<int>> CancelAuctionAsync(int auctionId, CancelAuctionRequestDto updateRequest);
        Task<Response<int>> SetWinnerAsync(int auctionId, SetWinnerRequestDto updateRequest);
    }
}