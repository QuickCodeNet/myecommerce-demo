using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Brand;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Brand
{
    public partial interface IBrandService
    {
        Task<Response<BrandDto>> InsertAsync(BrandDto request);
        Task<Response<bool>> DeleteAsync(BrandDto request);
        Task<Response<bool>> UpdateAsync(int id, BrandDto request);
        Task<Response<List<BrandDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<BrandDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool brandIsActive, int? page, int? size);
        Task<Response<List<SearchByNameResponseDto>>> SearchByNameAsync(string brandName, int? page, int? size);
        Task<Response<int>> DeactivateAsync(int brandId, DeactivateRequestDto updateRequest);
    }
}