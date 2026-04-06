using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.AuctionSettlement;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.AuctionSettlement
{
    public partial interface IAuctionSettlementService
    {
        Task<Response<AuctionSettlementDto>> InsertAsync(AuctionSettlementDto request);
        Task<Response<bool>> DeleteAsync(AuctionSettlementDto request);
        Task<Response<bool>> UpdateAsync(int id, AuctionSettlementDto request);
        Task<Response<List<AuctionSettlementDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<AuctionSettlementDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<GetByAuctionIdResponseDto>> GetByAuctionIdAsync(int auctionSettlementAuctionId);
        Task<Response<List<GetOutstandingPaymentsResponseDto>>> GetOutstandingPaymentsAsync(bool auctionSettlementIsPaid, int? page, int? size);
        Task<Response<int>> MarkAsPaidAsync(int auctionSettlementId, MarkAsPaidRequestDto updateRequest);
    }
}