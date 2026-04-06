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
    public partial interface IPaymentMethodService
    {
        Task<Response<PaymentMethodDto>> InsertAsync(PaymentMethodDto request);
        Task<Response<bool>> DeleteAsync(PaymentMethodDto request);
        Task<Response<bool>> UpdateAsync(int id, PaymentMethodDto request);
        Task<Response<List<PaymentMethodDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<PaymentMethodDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByCustomerIdResponseDto>>> GetByCustomerIdAsync(int paymentMethodCustomerId, bool paymentMethodIsActive, int? page, int? size);
        Task<Response<GetDefaultMethodResponseDto>> GetDefaultMethodAsync(int paymentMethodCustomerId, bool paymentMethodIsDefault, bool paymentMethodIsActive);
        Task<Response<int>> SetDefaultAsync(int paymentMethodId, int paymentMethodCustomerId, SetDefaultRequestDto updateRequest);
        Task<Response<int>> DeactivateAsync(int paymentMethodId, DeactivateRequestDto updateRequest);
    }
}