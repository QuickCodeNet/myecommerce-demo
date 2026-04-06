using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.Payment;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.Payment
{
    public partial class PaymentService : IPaymentService
    {
        private readonly ILogger<PaymentService> _logger;
        private readonly IPaymentRepository _repository;
        public PaymentService(ILogger<PaymentService> logger, IPaymentRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<PaymentDto>> InsertAsync(PaymentDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(PaymentDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, PaymentDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<PaymentDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<PaymentDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int paymentOrderId, int? page, int? size)
        {
            var returnValue = await _repository.GetByOrderIdAsync(paymentOrderId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetByReferenceIdResponseDto>> GetByReferenceIdAsync(Guid paymentReferenceId)
        {
            var returnValue = await _repository.GetByReferenceIdAsync(paymentReferenceId);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetByStatusResponseDto>>> GetByStatusAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetByStatusAsync(paymentStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetFailedPaymentsResponseDto>>> GetFailedPaymentsAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetFailedPaymentsAsync(paymentStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetPaymentsToCaptureResponseDto>>> GetPaymentsToCaptureAsync(PaymentStatus paymentStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetPaymentsToCaptureAsync(paymentStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetPaymentWithGatewayResponseDto>> GetPaymentWithGatewayAsync(int paymentsId, int paymentsPaymentGatewayId, int paymentGatewaysId)
        {
            var returnValue = await _repository.GetPaymentWithGatewayAsync(paymentsId, paymentsPaymentGatewayId, paymentGatewaysId);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetDailyVolumeResponseDto>> GetDailyVolumeAsync(PaymentStatus paymentStatus)
        {
            var returnValue = await _repository.GetDailyVolumeAsync(paymentStatus);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> UpdateStatusAsync(int paymentId, UpdateStatusRequestDto updateRequest)
        {
            var returnValue = await _repository.UpdateStatusAsync(paymentId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsCapturedAsync(int paymentId, MarkAsCapturedRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsCapturedAsync(paymentId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> MarkAsVoidedAsync(int paymentId, MarkAsVoidedRequestDto updateRequest)
        {
            var returnValue = await _repository.MarkAsVoidedAsync(paymentId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}