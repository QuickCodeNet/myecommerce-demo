using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Entities;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.OrderManagementModule.Application.Dtos.Address;
using QuickCode.MyecommerceDemo.OrderManagementModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.OrderManagementModule.Application.Services.Address
{
    public partial interface IAddressService
    {
        Task<Response<AddressDto>> InsertAsync(AddressDto request);
        Task<Response<bool>> DeleteAsync(AddressDto request);
        Task<Response<bool>> UpdateAsync(int id, AddressDto request);
        Task<Response<List<AddressDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<AddressDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int addressCustomerId, int? page, int? size);
        Task<Response<GetDefaultShippingResponseDto>> GetDefaultShippingAsync(int addressCustomerId, bool addressIsDefaultShipping);
        Task<Response<GetDefaultBillingResponseDto>> GetDefaultBillingAsync(int addressCustomerId, bool addressIsDefaultBilling);
        Task<Response<int>> SetDefaultShippingAsync(int addressId, int addressCustomerId, SetDefaultShippingRequestDto updateRequest);
    }
}