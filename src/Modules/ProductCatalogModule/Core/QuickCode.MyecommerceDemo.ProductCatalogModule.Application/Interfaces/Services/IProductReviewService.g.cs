using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Entities;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Dtos.ProductReview;
using QuickCode.MyecommerceDemo.ProductCatalogModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.ProductCatalogModule.Application.Services.ProductReview
{
    public partial interface IProductReviewService
    {
        Task<Response<ProductReviewDto>> InsertAsync(ProductReviewDto request);
        Task<Response<bool>> DeleteAsync(ProductReviewDto request);
        Task<Response<bool>> UpdateAsync(int id, ProductReviewDto request);
        Task<Response<List<ProductReviewDto>>> ListAsync(int? pageNumber, int? pageSize);
        Task<Response<ProductReviewDto>> GetItemAsync(int id);
        Task<Response<bool>> DeleteItemAsync(int id);
        Task<Response<int>> TotalItemCountAsync();
        Task<Response<List<GetByProductIdResponseDto>>> GetByProductIdAsync(int productReviewProductId, bool productReviewIsApproved, int? page, int? size);
        Task<Response<List<GetPendingApprovalResponseDto>>> GetPendingApprovalAsync(bool productReviewIsApproved, int? page, int? size);
        Task<Response<List<GetCustomerReviewsResponseDto>>> GetCustomerReviewsAsync(int productReviewCustomerId, int? page, int? size);
        Task<Response<GetAverageRatingResponseDto>> GetAverageRatingAsync(int productReviewProductId, bool productReviewIsApproved);
        Task<Response<int>> ApproveAsync(int productReviewId, ApproveRequestDto updateRequest);
    }
}