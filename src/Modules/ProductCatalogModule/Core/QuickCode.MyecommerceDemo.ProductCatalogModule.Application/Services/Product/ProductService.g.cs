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
    public partial class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;
        public ProductService(ILogger<ProductService> logger, IProductRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<ProductDto>> InsertAsync(ProductDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(ProductDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, ProductDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<ProductDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<ProductDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(ProductStatus productStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAsync(productStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetFeaturedResponseDto>>> GetFeaturedAsync(ProductStatus productStatus, bool productIsFeatured, int? page, int? size)
        {
            var returnValue = await _repository.GetFeaturedAsync(productStatus, productIsFeatured, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<SearchByNameResponseDto>>> SearchByNameAsync(string productName, int? page, int? size)
        {
            var returnValue = await _repository.SearchByNameAsync(productName, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<GetBySkuResponseDto>> GetBySkuAsync(string productSku)
        {
            var returnValue = await _repository.GetBySkuAsync(productSku);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetByCategoryResponseDto>>> GetByCategoryAsync(int productCategoryId, int? page, int? size)
        {
            var returnValue = await _repository.GetByCategoryAsync(productCategoryId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetLowStockResponseDto>>> GetLowStockAsync(ProductStatus productStatus, int? page, int? size)
        {
            var returnValue = await _repository.GetLowStockAsync(productStatus, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetProductsWithDetailsResponseDto>>> GetProductsWithDetailsAsync(int productsCategoryId, int productBrandId, int categoryId, int brandId, int? page, int? size)
        {
            var returnValue = await _repository.GetProductsWithDetailsAsync(productsCategoryId, productBrandId, categoryId, brandId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetRecentlyAddedResponseDto>>> GetRecentlyAddedAsync(DateTime productCreatedDate, int? page, int? size)
        {
            var returnValue = await _repository.GetRecentlyAddedAsync(productCreatedDate, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> UpdatePriceAsync(int productId, UpdatePriceRequestDto updateRequest)
        {
            var returnValue = await _repository.UpdatePriceAsync(productId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> AdjustStockAsync(int productId, AdjustStockRequestDto updateRequest)
        {
            var returnValue = await _repository.AdjustStockAsync(productId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> SetStatusAsync(int productId, SetStatusRequestDto updateRequest)
        {
            var returnValue = await _repository.SetStatusAsync(productId, updateRequest);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> ArchiveAsync(int productId, ArchiveRequestDto updateRequest)
        {
            var returnValue = await _repository.ArchiveAsync(productId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}