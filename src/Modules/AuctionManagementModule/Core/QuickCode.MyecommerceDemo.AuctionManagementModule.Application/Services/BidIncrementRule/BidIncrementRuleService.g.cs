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
    public partial class BidIncrementRuleService : IBidIncrementRuleService
    {
        private readonly ILogger<BidIncrementRuleService> _logger;
        private readonly IBidIncrementRuleRepository _repository;
        public BidIncrementRuleService(ILogger<BidIncrementRuleService> logger, IBidIncrementRuleRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<BidIncrementRuleDto>> InsertAsync(BidIncrementRuleDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(BidIncrementRuleDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, BidIncrementRuleDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<BidIncrementRuleDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<BidIncrementRuleDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveRulesResponseDto>>> GetActiveRulesAsync(bool bidIncrementRuleIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveRulesAsync(bidIncrementRuleIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetIncrementForPriceResponseDto>> GetIncrementForPriceAsync(bool bidIncrementRuleIsActive)
        {
            var returnValue = await _repository.GetIncrementForPriceAsync(bidIncrementRuleIsActive);
            return returnValue.ToResponse();
        }
    }
}