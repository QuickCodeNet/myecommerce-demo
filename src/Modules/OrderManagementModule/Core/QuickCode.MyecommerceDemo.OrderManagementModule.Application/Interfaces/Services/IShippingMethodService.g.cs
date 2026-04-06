using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.ShippingMethod;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.ShippingMethod
{
    public partial interface IShippingMethodService
    {
        Task<Response<ShippingMethodDto>> InsertAsync(ShippingMethodDto request);
        Task<Response<bool>> DeleteAsync(ShippingMethodDto request);
        Task<Response<bool>> UpdateAsync(int id, ShippingMethodDto request);
        Task<Response<List<ShippingMethodDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<ShippingMethodDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool shippingMethodIsActive, int? page, int? size);
        Task<Response<int>> DeactivateAsync(int shippingMethodId, DeactivateRequestDto updateRequest);
    }
}