using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Entities;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Dtos.GatewayConfig;
using QuickCode.MyecommerceDemo.PaymentProcessingModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.PaymentProcessingModule.Application.Services.GatewayConfig
{
    public partial interface IGatewayConfigService
    {
        Task<Response<GatewayConfigDto>> InsertAsync(GatewayConfigDto request);
        Task<Response<bool>> DeleteAsync(GatewayConfigDto request);
        Task<Response<bool>> UpdateAsync(int id, GatewayConfigDto request);
        Task<Response<List<GatewayConfigDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<GatewayConfigDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByGatewayIdResponseDto>>> GetByGatewayIdAsync(int gatewayConfigGatewayId, bool gatewayConfigIsSecret, int? page, int? size);
    }
}