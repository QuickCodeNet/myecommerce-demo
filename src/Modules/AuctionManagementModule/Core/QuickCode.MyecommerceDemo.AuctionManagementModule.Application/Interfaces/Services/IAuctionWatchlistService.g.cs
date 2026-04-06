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
    public partial interface IAuctionWatchlistService
    {
        Task<Response<AuctionWatchlistDto>> InsertAsync(AuctionWatchlistDto request);
        Task<Response<bool>> DeleteAsync(AuctionWatchlistDto request);
        Task<Response<bool>> UpdateAsync(int id, AuctionWatchlistDto request);
        Task<Response<List<AuctionWatchlistDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<AuctionWatchlistDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByUserIdResponseDto>>> GetByUserIdAsync(int auctionWatchlistUserId, int? page, int? size);
        Task<Response<List<GetWatchersByAuctionResponseDto>>> GetWatchersByAuctionAsync(int auctionWatchlistAuctionId, int? page, int? size);
        Task<Response<bool>> IsWatchingAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId);
        Task<Response<int>> UnwatchAsync(int auctionWatchlistAuctionId, int auctionWatchlistUserId);
    }
}