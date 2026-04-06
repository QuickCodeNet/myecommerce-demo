using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.OrderStatusHistory;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.OrderStatusHistory
{
    public partial class OrderStatusHistoryService : IOrderStatusHistoryService
    {
        private readonly ILogger<OrderStatusHistoryService> _logger;
        private readonly IOrderStatusHistoryRepository _repository;
        public OrderStatusHistoryService(ILogger<OrderStatusHistoryService> logger, IOrderStatusHistoryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<OrderStatusHistoryDto>> InsertAsync(OrderStatusHistoryDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(OrderStatusHistoryDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, OrderStatusHistoryDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<OrderStatusHistoryDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<OrderStatusHistoryDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int orderStatusHistoryOrderId, int? page, int? size)
        {
            var returnValue = await _repository.GetByOrderIdAsync(orderStatusHistoryOrderId, page, size);
            return returnValue.ToResponse();
        }
    }
}