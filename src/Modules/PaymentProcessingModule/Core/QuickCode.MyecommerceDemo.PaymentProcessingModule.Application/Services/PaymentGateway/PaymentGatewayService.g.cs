using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.PaymentGateway;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.PaymentGateway
{
    public partial class PaymentGatewayService : IPaymentGatewayService
    {
        private readonly ILogger<PaymentGatewayService> _logger;
        private readonly IPaymentGatewayRepository _repository;
        public PaymentGatewayService(ILogger<PaymentGatewayService> logger, IPaymentGatewayRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<PaymentGatewayDto>> InsertAsync(PaymentGatewayDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(PaymentGatewayDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, PaymentGatewayDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<PaymentGatewayDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<PaymentGatewayDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool paymentGatewayIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAsync(paymentGatewayIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> DeactivateAsync(int paymentGatewayId, DeactivateRequestDto updateRequest)
        {
            var returnValue = await _repository.DeactivateAsync(paymentGatewayId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}