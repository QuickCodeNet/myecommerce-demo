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
    public partial class AuctionItemService : IAuctionItemService
    {
        private readonly ILogger<AuctionItemService> _logger;
        private readonly IAuctionItemRepository _repository;
        public AuctionItemService(ILogger<AuctionItemService> logger, IAuctionItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<AuctionItemDto>> InsertAsync(AuctionItemDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(AuctionItemDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, AuctionItemDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<AuctionItemDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<AuctionItemDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetAvailableByOwnerResponseDto>>> GetAvailableByOwnerAsync(int auctionItemOwnerId, bool auctionItemIsAvailable, int? page, int? size)
        {
            var returnValue = await _repository.GetAvailableByOwnerAsync(auctionItemOwnerId, auctionItemIsAvailable, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsUnavailableAsync(int auctionItemId, MarkAsUnavailableRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsUnavailableAsync(auctionItemId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}