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
    public partial class AuctionSettlementService : IAuctionSettlementService
    {
        private readonly ILogger<AuctionSettlementService> _logger;
        private readonly IAuctionSettlementRepository _repository;
        public AuctionSettlementService(ILogger<AuctionSettlementService> logger, IAuctionSettlementRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<AuctionSettlementDto>> InsertAsync(AuctionSettlementDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(AuctionSettlementDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, AuctionSettlementDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<AuctionSettlementDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<AuctionSettlementDto>> GetItemAsync(int id)
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

        public async Task<Response<GetByAuctionIdResponseDto>> GetByAuctionIdAsync(int auctionSettlementAuctionId)
        {
            var returnValue = await _repository.GetByAuctionIdAsync(auctionSettlementAuctionId);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetOutstandingPaymentsResponseDto>>> GetOutstandingPaymentsAsync(bool auctionSettlementIsPaid, int? page, int? size)
        {
            var returnValue = await _repository.GetOutstandingPaymentsAsync(auctionSettlementIsPaid, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsPaidAsync(int auctionSettlementId, MarkAsPaidRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsPaidAsync(auctionSettlementId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}