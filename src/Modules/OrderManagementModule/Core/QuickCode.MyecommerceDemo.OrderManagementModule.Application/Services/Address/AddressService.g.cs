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
    public partial class AddressService : IAddressService
    {
        private readonly ILogger<AddressService> _logger;
        private readonly IAddressRepository _repository;
        public AddressService(ILogger<AddressService> logger, IAddressRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<AddressDto>> InsertAsync(AddressDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(AddressDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, AddressDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<AddressDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<AddressDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int addressCustomerId, int? page, int? size)
        {
            var returnValue = await _repository.GetByCustomerIdAsync(addressCustomerId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetDefaultShippingResponseDto>> GetDefaultShippingAsync(int addressCustomerId, bool addressIsDefaultShipping)
        {
            var returnValue = await _repository.GetDefaultShippingAsync(addressCustomerId, addressIsDefaultShipping);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetDefaultBillingResponseDto>> GetDefaultBillingAsync(int addressCustomerId, bool addressIsDefaultBilling)
        {
            var returnValue = await _repository.GetDefaultBillingAsync(addressCustomerId, addressIsDefaultBilling);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> SetDefaultShippingAsync(int addressId, int addressCustomerId, SetDefaultShippingRequestDto updateRequest)
        {
            var returnValue = await _repository.SetDefaultShippingAsync(addressId, addressCustomerId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}