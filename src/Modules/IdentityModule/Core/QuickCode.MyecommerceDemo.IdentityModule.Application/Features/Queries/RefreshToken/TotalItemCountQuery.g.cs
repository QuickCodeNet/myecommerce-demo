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
    public class TotalCountRefreshTokenQuery : IRequest<Response<int>>
    {
        public TotalCountRefreshTokenQuery()
        {
        }

        public class TotalCountRefreshTokenHandler : IRequestHandler<TotalCountRefreshTokenQuery, Response<int>>
        {
            private readonly ILogger<TotalCountRefreshTokenHandler> _logger;
            private readonly IRefreshTokenRepository _repository;
            public TotalCountRefreshTokenHandler(ILogger<TotalCountRefreshTokenHandler> logger, IRefreshTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountRefreshTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}