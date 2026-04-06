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
    public partial class RefundService : IRefundService
    {
        private readonly ILogger<RefundService> _logger;
        private readonly IRefundRepository _repository;
        public RefundService(ILogger<RefundService> logger, IRefundRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<RefundDto>> InsertAsync(RefundDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(RefundDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, RefundDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<RefundDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<RefundDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByPaymentIdResponseDto>>> GetByPaymentIdAsync(int refundPaymentId, int? page, int? size)
        {
            var returnValue = await _repository.GetByPaymentIdAsync(refundPaymentId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetPendingRefundsResponseDto>>> GetPendingRefundsAsync(RefundStatus refundStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetPendingRefundsAsync(refundStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> ApproveAsync(int refundId, ApproveRequestDto updateRequest)
        {
            var returnValue = await _repository.ApproveAsync(refundId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> ProcessAsync(int refundId, ProcessRequestDto updateRequest)
        {
            var returnValue = await _repository.ProcessAsync(refundId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> RejectAsync(int refundId, RejectRequestDto updateRequest)
        {
            var returnValue = await _repository.RejectAsync(refundId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}