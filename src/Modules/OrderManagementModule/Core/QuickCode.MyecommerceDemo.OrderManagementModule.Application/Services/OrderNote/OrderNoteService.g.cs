using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.OrderNote;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.OrderNote
{
    public partial class OrderNoteService : IOrderNoteService
    {
        private readonly ILogger<OrderNoteService> _logger;
        private readonly IOrderNoteRepository _repository;
        public OrderNoteService(ILogger<OrderNoteService> logger, IOrderNoteRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<OrderNoteDto>> InsertAsync(OrderNoteDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(OrderNoteDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, OrderNoteDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<OrderNoteDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<OrderNoteDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int orderNoteOrderId, int? page, int? size)
        {
            var returnValue = await _repository.GetByOrderIdAsync(orderNoteOrderId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetCustomerVisibleNotesResponseDto>>> GetCustomerVisibleNotesAsync(int orderNoteOrderId, bool orderNoteIsCustomerVisible, int? page, int? size)
        {
            var returnValue = await _repository.GetCustomerVisibleNotesAsync(orderNoteOrderId, orderNoteIsCustomerVisible, page, size);
            return returnValue.ToResponse();
        }
    }
}