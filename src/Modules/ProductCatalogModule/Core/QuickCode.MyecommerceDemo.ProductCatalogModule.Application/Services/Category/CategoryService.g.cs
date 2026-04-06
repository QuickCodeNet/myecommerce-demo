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
    public partial class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _repository;
        public CategoryService(ILogger<CategoryService> logger, ICategoryRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<CategoryDto>> InsertAsync(CategoryDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(CategoryDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, CategoryDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<CategoryDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<CategoryDto>> GetItemAsync(int id)
        {
            var returnValue = await _repository.GetByPkAsync(id);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteItemAsync(int id)
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

        public async Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool categoryIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAsync(categoryIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetBySlugResponseDto>> GetBySlugAsync(string categorySlug)
        {
            var returnValue = await _repository.GetBySlugAsync(categorySlug);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetSubCategoriesResponseDto>>> GetSubCategoriesAsync(int categoryParentCategoryId, int? page, int? size)
        {
            var returnValue = await _repository.GetSubCategoriesAsync(categoryParentCategoryId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetRootCategoriesResponseDto>>> GetRootCategoriesAsync(int categoryParentCategoryId, int? page, int? size)
        {
            var returnValue = await _repository.GetRootCategoriesAsync(categoryParentCategoryId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> DeactivateAsync(int categoryId, DeactivateRequestDto updateRequest)
        {
            var returnValue = await _repository.DeactivateAsync(categoryId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}