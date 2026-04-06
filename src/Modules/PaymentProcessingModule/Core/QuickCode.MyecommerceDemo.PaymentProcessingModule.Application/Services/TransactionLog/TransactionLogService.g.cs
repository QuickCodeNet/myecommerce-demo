using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.TransactionLog;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.TransactionLog
{
    public partial class TransactionLogService : ITransactionLogService
    {
        private readonly ILogger<TransactionLogService> _logger;
        private readonly ITransactionLogRepository _repository;
        public TransactionLogService(ILogger<TransactionLogService> logger, ITransactionLogRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<TransactionLogDto>> InsertAsync(TransactionLogDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(TransactionLogDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(long id, TransactionLogDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<TransactionLogDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<TransactionLogDto>> GetItemAsync(long id)
        {
            var returnValue = await _repository.GetByPkAsync(id);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteItemAsync(long id)
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

        public async Task<Response<List<GetByPaymentIdResponseDto>>> GetByPaymentIdAsync(int transactionLogPaymentId, int? page, int? size)
        {
            var returnValue = await _repository.GetByPaymentIdAsync(transactionLogPaymentId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<SearchLogsResponseDto>>> SearchLogsAsync(string transactionLogMessage, int? page, int? size)
        {
            var returnValue = await _repository.SearchLogsAsync(transactionLogMessage, page, size);
            return returnValue.ToResponse();
        }
    }
}