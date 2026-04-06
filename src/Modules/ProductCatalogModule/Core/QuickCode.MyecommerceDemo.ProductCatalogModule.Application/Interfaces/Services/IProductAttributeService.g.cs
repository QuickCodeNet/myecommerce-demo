using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductAttribute;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductAttribute
{
    public partial interface IProductAttributeService
    {
        Task<Response<ProductAttributeDto>> InsertAsync(ProductAttributeDto request);
        Task<Response<bool>> DeleteAsync(ProductAttributeDto request);
        Task<Response<bool>> UpdateAsync(int id, ProductAttributeDto request);
        Task<Response<List<ProductAttributeDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<ProductAttributeDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
    }
}