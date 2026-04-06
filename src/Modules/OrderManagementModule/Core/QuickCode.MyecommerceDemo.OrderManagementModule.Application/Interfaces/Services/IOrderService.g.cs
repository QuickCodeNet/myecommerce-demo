using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.Order;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.Order
{
    public partial interface IOrderService
    {
        Task<Response<OrderDto>> InsertAsync(OrderDto request);
        Task<Response<bool>> DeleteAsync(OrderDto request);
        Task<Response<bool>> UpdateAsync(int id, OrderDto request);
        Task<Response<List<OrderDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<OrderDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int orderCustomerId, int? page, int? size);
        Task<Response<GetByOrderNumberResponseDto>> GetByOrderNumberAsync(string orderOrderNumber);
        Task<Response<List<GetByStatusResponseDto>>> GetByStatusAsync(OrderStatus orderStatus, int? page, int? size);
        Task<Response<List<GetRecentOrdersResponseDto>>> GetRecentOrdersAsync(DateTime orderCreatedDate, int? page, int? size);
        Task<Response<List<GetOrdersForFulfillmentResponseDto>>> GetOrdersForFulfillmentAsync(OrderStatus orderStatus, int? page, int? size);
        Task<Response<GetOrderWithDetailsResponseDto>> GetOrderWithDetailsAsync(int ordersId, int orderShippingAddressId, int orderBillingAddressId, int addressId);
        Task<Response<GetMonthlyRevenueResponseDto>> GetMonthlyRevenueAsync(DateTime orderCreatedDate);
        Task<Response<int>> UpdateStatusAsync(int orderId, UpdateStatusRequestDto updateRequest);
        Task<Response<int>> MarkAsPaidAsync(int orderId, MarkAsPaidRequestDto updateRequest);
        Task<Response<int>> MarkAsShippedAsync(int orderId, MarkAsShippedRequestDto updateRequest);
        Task<Response<int>> CancelOrderAsync(int orderId, CancelOrderRequestDto updateRequest);
    }
}