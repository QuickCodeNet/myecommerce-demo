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
    public partial class ProductAttributeValueService : IProductAttributeValueService
    {
        private readonly ILogger<ProductAttributeValueService> _logger;
        private readonly IProductAttributeValueRepository _repository;
        public ProductAttributeValueService(ILogger<ProductAttributeValueService> logger, IProductAttributeValueRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<ProductAttributeValueDto>> InsertAsync(ProductAttributeValueDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(ProductAttributeValueDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, ProductAttributeValueDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<ProductAttributeValueDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<ProductAttributeValueDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetByProductIdResponseDto>>> GetByProductIdAsync(int productAttributeValueProductId, int? page, int? size)
        {
            var returnValue = await _repository.GetByProductIdAsync(productAttributeValueProductId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<GetAttributesForProductResponseDto>>> GetAttributesForProductAsync(int productAttributeValuesProductId, int productAttributeValuesAttributeId, int productAttributesId, int? page, int? size)
        {
            var returnValue = await _repository.GetAttributesForProductAsync(productAttributeValuesProductId, productAttributeValuesAttributeId, productAttributesId, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> RemoveByProductAsync(int productAttributeValueProductId)
        {
            var returnValue = await _repository.RemoveByProductAsync(productAttributeValueProductId);
            return returnValue.ToResponse();
        }
    }
}