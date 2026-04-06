using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductAttributeValue;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductAttributeValue
{
    public partial interface IProductAttributeValueService
    {
        Task<Response<ProductAttributeValueDto>> InsertAsync(ProductAttributeValueDto request);
        Task<Response<bool>> DeleteAsync(ProductAttributeValueDto request);
        Task<Response<bool>> UpdateAsync(int id, ProductAttributeValueDto request);
        Task<Response<List<ProductAttributeValueDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<ProductAttributeValueDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByProductIdResponseDto>>> GetByProductIdAsync(int productAttributeValueProductId, int? page, int? size);
        Task<Response<List<GetAttributesForProductResponseDto>>> GetAttributesForProductAsync(int productAttributeValuesProductId, int productAttributeValuesAttributeId, int productAttributesId, int? page, int? size);
        Task<Response<int>> RemoveByProductAsync(int productAttributeValueProductId);
    }
}