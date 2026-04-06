using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Product;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Product
{
    public partial interface IProductService
    {
        Task<Response<ProductDto>> InsertAsync(ProductDto request);
        Task<Response<bool>> DeleteAsync(ProductDto request);
        Task<Response<bool>> UpdateAsync(int id, ProductDto request);
        Task<Response<List<ProductDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<ProductDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(ProductStatus productStatus, int? page, int? size);
        Task<Response<List<GetFeaturedResponseDto>>> GetFeaturedAsync(ProductStatus productStatus, bool productIsFeatured, int? page, int? size);
        Task<Response<List<SearchByNameResponseDto>>> SearchByNameAsync(string productName, int? page, int? size);
        Task<Response<GetBySkuResponseDto>> GetBySkuAsync(string productSku);
        Task<Response<List<GetByCategoryResponseDto>>> GetByCategoryAsync(int productCategoryId, int? page, int? size);
        Task<Response<List<GetLowStockResponseDto>>> GetLowStockAsync(ProductStatus productStatus, int? page, int? size);
        Task<Response<List<GetProductsWithDetailsResponseDto>>> GetProductsWithDetailsAsync(int productsCategoryId, int productBrandId, int categoryId, int brandId, int? page, int? size);
        Task<Response<List<GetRecentlyAddedResponseDto>>> GetRecentlyAddedAsync(DateTime productCreatedDate, int? page, int? size);
        Task<Response<int>> UpdatePriceAsync(int productId, UpdatePriceRequestDto updateRequest);
        Task<Response<int>> AdjustStockAsync(int productId, AdjustStockRequestDto updateRequest);
        Task<Response<int>> SetStatusAsync(int productId, SetStatusRequestDto updateRequest);
        Task<Response<int>> ArchiveAsync(int productId, ArchiveRequestDto updateRequest);
    }
}