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
    public partial interface IPaymentService
    {
        Task<Response<PaymentDto>> InsertAsync(PaymentDto request);
        Task<Response<bool>> DeleteAsync(PaymentDto request);
        Task<Response<bool>> UpdateAsync(int id, PaymentDto request);
        Task<Response<List<PaymentDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<PaymentDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByOrderIdResponseDto>>> GetByOrderIdAsync(int paymentOrderId, int? page, int? size);
        Task<Response<GetByReferenceIdResponseDto>> GetByReferenceIdAsync(Guid paymentReferenceId);
        Task<Response<List<GetByStatusResponseDto>>> GetByStatusAsync(PaymentStatus paymentStatus, int? page, int? size);
        Task<Response<List<GetFailedPaymentsResponseDto>>> GetFailedPaymentsAsync(PaymentStatus paymentStatus, int? page, int? size);
        Task<Response<List<GetPaymentsToCaptureResponseDto>>> GetPaymentsToCaptureAsync(PaymentStatus paymentStatus, int? page, int? size);
        Task<Response<GetPaymentWithGatewayResponseDto>> GetPaymentWithGatewayAsync(int paymentsId, int paymentsPaymentGatewayId, int paymentGatewaysId);
        Task<Response<GetDailyVolumeResponseDto>> GetDailyVolumeAsync(PaymentStatus paymentStatus);
        Task<Response<int>> UpdateStatusAsync(int paymentId, UpdateStatusRequestDto updateRequest);
        Task<Response<int>> MarkAsCapturedAsync(int paymentId, MarkAsCapturedRequestDto updateRequest);
        Task<Response<int>> MarkAsVoidedAsync(int paymentId, MarkAsVoidedRequestDto updateRequest);
    }
}