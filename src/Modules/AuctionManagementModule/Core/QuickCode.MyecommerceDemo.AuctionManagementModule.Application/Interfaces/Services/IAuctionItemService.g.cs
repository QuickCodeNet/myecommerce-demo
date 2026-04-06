using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionItem;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionItem
{
    public partial interface IAuctionItemService
    {
        Task<Response<AuctionItemDto>> InsertAsync(AuctionItemDto request);
        Task<Response<bool>> DeleteAsync(AuctionItemDto request);
        Task<Response<bool>> UpdateAsync(int id, AuctionItemDto request);
        Task<Response<List<AuctionItemDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<AuctionItemDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetAvailableByOwnerResponseDto>>> GetAvailableByOwnerAsync(int auctionItemOwnerId, bool auctionItemIsAvailable, int? page, int? size);
        Task<Response<int>> MarkAsUnavailableAsync(int auctionItemId, MarkAsUnavailableRequestDto updateRequest);
    }
}