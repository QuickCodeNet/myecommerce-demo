using System;
using System.Linq;
using QuickCode.MyecommerceDemo.Common.Mediator;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.RefreshToken;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.RefreshToken
{
    public class ListRefreshTokenQuery : IRequest<Response<List<RefreshTokenDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListRefreshTokenQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListRefreshTokenHandler : IRequestHandler<ListRefreshTokenQuery, Response<List<RefreshTokenDto>>>
        {
            private readonly ILogger<ListRefreshTokenHandler> _logger;
            private readonly IRefreshTokenRepository _repository;
            public ListRefreshTokenHandler(ILogger<ListRefreshTokenHandler> logger, IRefreshTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<RefreshTokenDto>>> Handle(ListRefreshTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}