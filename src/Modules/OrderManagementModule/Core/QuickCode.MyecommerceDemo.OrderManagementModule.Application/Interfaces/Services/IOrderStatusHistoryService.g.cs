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
    public partial interface IOrderStatusHistoryService
    {
        Task<Response<OrderStatusHistoryDto>> InsertAsync(OrderStatusHistoryDto request);
        Task<Response<bool>> DeleteAsync(OrderStatusHistoryDto request);
        Task<Response<bool>> UpdateAsync(int id, OrderStatusHistoryDto request);
        Task<Response<List<OrderStatusHistoryDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<OrderStatusHistoryDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int orderStatusHistoryOrderId, int? page, int? size);
    }
}