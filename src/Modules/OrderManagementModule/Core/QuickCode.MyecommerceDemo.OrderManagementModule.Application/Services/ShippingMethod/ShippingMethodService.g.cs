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
    public partial class ShippingMethodService : IShippingMethodService
    {
        private readonly ILogger<ShippingMethodService> _logger;
        private readonly IShippingMethodRepository _repository;
        public ShippingMethodService(ILogger<ShippingMethodService> logger, IShippingMethodRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<ShippingMethodDto>> InsertAsync(ShippingMethodDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(ShippingMethodDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, ShippingMethodDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<ShippingMethodDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<ShippingMethodDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool shippingMethodIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAsync(shippingMethodIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> DeactivateAsync(int shippingMethodId, DeactivateRequestDto updateRequest)
        {
            var returnValue = await _repository.DeactivateAsync(shippingMethodId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}