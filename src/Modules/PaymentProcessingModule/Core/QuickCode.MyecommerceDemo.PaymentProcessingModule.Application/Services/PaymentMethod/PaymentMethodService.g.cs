using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.PaymentMethod;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.PaymentMethod
{
    public partial class PaymentMethodService : IPaymentMethodService
    {
        private readonly ILogger<PaymentMethodService> _logger;
        private readonly IPaymentMethodRepository _repository;
        public PaymentMethodService(ILogger<PaymentMethodService> logger, IPaymentMethodRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<PaymentMethodDto>> InsertAsync(PaymentMethodDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(PaymentMethodDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, PaymentMethodDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<PaymentMethodDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<PaymentMethodDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int paymentMethodCustomerId, bool paymentMethodIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetByCustomerIdAsync(paymentMethodCustomerId, paymentMethodIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetDefaultMethodResponseDto>> GetDefaultMethodAsync(int paymentMethodCustomerId, bool paymentMethodIsDefault, bool paymentMethodIsActive)
        {
            var returnValue = await _repository.GetDefaultMethodAsync(paymentMethodCustomerId, paymentMethodIsDefault, paymentMethodIsActive);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> SetDefaultAsync(int paymentMethodId, int paymentMethodCustomerId, SetDefaultRequestDto updateRequest)
        {
            var returnValue = await _repository.SetDefaultAsync(paymentMethodId, paymentMethodCustomerId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> DeactivateAsync(int paymentMethodId, DeactivateRequestDto updateRequest)
        {
            var returnValue = await _repository.DeactivateAsync(paymentMethodId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}