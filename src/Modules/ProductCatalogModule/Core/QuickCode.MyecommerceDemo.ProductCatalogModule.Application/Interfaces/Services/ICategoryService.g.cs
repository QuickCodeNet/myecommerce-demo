using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.Category;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.Category
{
    public partial interface ICategoryService
    {
        Task<Response<CategoryDto>> InsertAsync(CategoryDto request);
        Task<Response<bool>> DeleteAsync(CategoryDto request);
        Task<Response<bool>> UpdateAsync(int id, CategoryDto request);
        Task<Response<List<CategoryDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<CategoryDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool categoryIsActive, int? page, int? size);
        Task<Response<GetBySlugResponseDto>> GetBySlugAsync(string categorySlug);
        Task<Response<List<GetSubCategoriesResponseDto>>> GetSubCategoriesAsync(int categoryParentCategoryId, int? page, int? size);
        Task<Response<List<GetRootCategoriesResponseDto>>> GetRootCategoriesAsync(int categoryParentCategoryId, int? page, int? size);
        Task<Response<int>> DeactivateAsync(int categoryId, DeactivateRequestDto updateRequest);
    }
}