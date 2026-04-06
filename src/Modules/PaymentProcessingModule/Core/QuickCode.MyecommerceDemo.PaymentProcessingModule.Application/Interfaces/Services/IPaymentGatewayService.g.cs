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
    public partial interface IPaymentGatewayService
    {
        Task<Response<PaymentGatewayDto>> InsertAsync(PaymentGatewayDto request);
        Task<Response<bool>> DeleteAsync(PaymentGatewayDto request);
        Task<Response<bool>> UpdateAsync(int id, PaymentGatewayDto request);
        Task<Response<List<PaymentGatewayDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<PaymentGatewayDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool paymentGatewayIsActive, int? page, int? size);
        Task<Response<int>> DeactivateAsync(int paymentGatewayId, DeactivateRequestDto updateRequest);
    }
}