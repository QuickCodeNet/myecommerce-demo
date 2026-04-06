using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.Refund;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.Refund
{
    public partial interface IRefundService
    {
        Task<Response<RefundDto>> InsertAsync(RefundDto request);
        Task<Response<bool>> DeleteAsync(RefundDto request);
        Task<Response<bool>> UpdateAsync(int id, RefundDto request);
        Task<Response<List<RefundDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<RefundDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByPaymentIdResponseDto>>> GetByPaymentIdAsync(int refundPaymentId, int? page, int? size);
        Task<Response<List<GetPendingRefundsResponseDto>>> GetPendingRefundsAsync(RefundStatus refundStatus, int? page, int? size);
        Task<Response<int>> ApproveAsync(int refundId, ApproveRequestDto updateRequest);
        Task<Response<int>> ProcessAsync(int refundId, ProcessRequestDto updateRequest);
        Task<Response<int>> RejectAsync(int refundId, RejectRequestDto updateRequest);
    }
}