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
    public partial interface IBidService
    {
        Task<Response<BidDto>> InsertAsync(BidDto request);
        Task<Response<bool>> DeleteAsync(BidDto request);
        Task<Response<bool>> UpdateAsync(int id, BidDto request);
        Task<Response<List<BidDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<BidDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByAuctionIdResponseDto>>> GetByAuctionIdAsync(int bidAuctionId, int? page, int? size);
        Task<Response<List<GetByBidderIdResponseDto>>> GetByBidderIdAsync(int bidBidderId, int? page, int? size);
        Task<Response<GetHighestBidResponseDto>> GetHighestBidAsync(int bidAuctionId);
        Task<Response<int>> MarkAsOutbidAsync(int bidId, MarkAsOutbidRequestDto updateRequest);
        Task<Response<int>> MarkAsWinningAsync(int bidId, MarkAsWinningRequestDto updateRequest);
    }
}