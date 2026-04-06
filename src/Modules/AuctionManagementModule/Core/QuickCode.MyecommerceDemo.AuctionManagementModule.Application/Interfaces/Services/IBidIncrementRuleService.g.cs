using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Dtos.BidIncrementRule;
using QuickCode.MyecommerceDemo.AuctionManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.AuctionManagementModule.Application.Services.BidIncrementRule
{
    public partial interface IBidIncrementRuleService
    {
        Task<Response<BidIncrementRuleDto>> InsertAsync(BidIncrementRuleDto request);
        Task<Response<bool>> DeleteAsync(BidIncrementRuleDto request);
        Task<Response<bool>> UpdateAsync(int id, BidIncrementRuleDto request);
        Task<Response<List<BidIncrementRuleDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<BidIncrementRuleDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveRulesResponseDto>>> GetActiveRulesAsync(bool bidIncrementRuleIsActive, int? page, int? size);
        Task<Response<GetIncrementForPriceResponseDto>> GetIncrementForPriceAsync(bool bidIncrementRuleIsActive);
    }
}