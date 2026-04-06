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
    public partial class BrandService : IBrandService
    {
        private readonly ILogger<BrandService> _logger;
        private readonly IBrandRepository _repository;
        public BrandService(ILogger<BrandService> logger, IBrandRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<Response<BrandDto>> InsertAsync(BrandDto request)
        {
            var returnValue = await _repository.InsertAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> DeleteAsync(BrandDto request)
        {
            var returnValue = await _repository.DeleteAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<bool>> UpdateAsync(int id, BrandDto request)
        {
            var updateItem = await _repository.GetByPkAsync(request.Id);
            if (updateItem.Code == 404)
                return Response<bool>.NotFound();
            var returnValue = await _repository.UpdateAsync(request);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<BrandDto>>> ListAsync(int? pageNumber, int? pageSize)
        {
            var returnValue = await _repository.ListAsync(pageNumber, pageSize);
            return returnValue.ToResponse();
        }

        public async Task<Response<BrandDto>> GetItemAsync(int id)
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

        public async Task<Response<List<GetActiveResponseDto>>> GetActiveAsync(bool brandIsActive, int? page, int? size)
        {
            var returnValue = await _repository.GetActiveAsync(brandIsActive, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<List<SearchByNameResponseDto>>> SearchByNameAsync(string brandName, int? page, int? size)
        {
            var returnValue = await _repository.SearchByNameAsync(brandName, page, size);
            return returnValue.ToResponse();
        }

        public async Task<Response<int>> DeactivateAsync(int brandId, DeactivateRequestDto updateRequest)
        {
            var returnValue = await _repository.DeactivateAsync(brandId, updateRequest);
            return returnValue.ToResponse();
        }
    }
}