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
    public partial interface ITransactionLogService
    {
        Task<Response<TransactionLogDto>> InsertAsync(TransactionLogDto request);
        Task<Response<bool>> DeleteAsync(TransactionLogDto request);
        Task<Response<bool>> UpdateAsync(long id, TransactionLogDto request);
        Task<Response<List<TransactionLogDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<TransactionLogDto>> GetItemAsync(long id);
        Task<Response<bool>> DeleteItemAsync(long id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByPaymentIdResponseDto>>> GetByPaymentIdAsync(int transactionLogPaymentId, int? page, int? size);
        Task<Response<List<SearchLogsResponseDto>>> SearchLogsAsync(string transactionLogMessage, int? page, int? size);
    }
}