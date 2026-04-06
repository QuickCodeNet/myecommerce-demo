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
    public partial interface IOrderNoteService
    {
        Task<Response<OrderNoteDto>> InsertAsync(OrderNoteDto request);
        Task<Response<bool>> DeleteAsync(OrderNoteDto request);
        Task<Response<bool>> UpdateAsync(int id, OrderNoteDto request);
        Task<Response<List<OrderNoteDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<OrderNoteDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int orderNoteOrderId, int? page, int? size);
        Task<Response<List<GetCustomerVisibleNotesResponseDto>>> GetCustomerVisibleNotesAsync(int orderNoteOrderId, bool orderNoteIsCustomerVisible, int? page, int? size);
    }
}