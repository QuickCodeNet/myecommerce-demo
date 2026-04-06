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
    public partial class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IOrderRepository _repository;
        public OrderService(ILogger<OrderService> logger, IOrderRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<OrderDto>> InsertAsync(OrderDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(OrderDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, OrderDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<OrderDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<OrderDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int orderCustomerId, int? page, int? size)
        {
            var returnValue = await _repository.GetByCustomerIdAsync(orderCustomerId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetByOrderNumberResponseDto>> GetByOrderNumberAsync(string orderOrderNumber)
        {
            var returnValue = await _repository.GetByOrderNumberAsync(orderOrderNumber);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetByStatusResponseDto>>> GetByStatusAsync(OrderStatus orderStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetByStatusAsync(orderStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetRecentOrdersResponseDto>>> GetRecentOrdersAsync(DateTime orderCreatedDate, int? page, int? size)
        {
            var returnValue = await _repository.GetRecentOrdersAsync(orderCreatedDate, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetOrdersForFulfillmentResponseDto>>> GetOrdersForFulfillmentAsync(OrderStatus orderStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetOrdersForFulfillmentAsync(orderStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetOrderWithDetailsResponseDto>> GetOrderWithDetailsAsync(int ordersId, int orderShippingAddressId, int orderBillingAddressId, int addressId)
        {
            var returnValue = await _repository.GetOrderWithDetailsAsync(ordersId, orderShippingAddressId, orderBillingAddressId, addressId);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetMonthlyRevenueResponseDto>> GetMonthlyRevenueAsync(DateTime orderCreatedDate)
        {
            var returnValue = await _repository.GetMonthlyRevenueAsync(orderCreatedDate);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> UpdateStatusAsync(int orderId, UpdateStatusRequestDto updateRequest)
        {
            var returnValue = await _repository.UpdateStatusAsync(orderId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsPaidAsync(int orderId, MarkAsPaidRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsPaidAsync(orderId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsShippedAsync(int orderId, MarkAsShippedRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsShippedAsync(orderId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> CancelOrderAsync(int orderId, CancelOrderRequestDto updateRequest)
        {
            var returnValue = await _repository.CancelOrderAsync(orderId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}