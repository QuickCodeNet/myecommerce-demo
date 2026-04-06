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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserToken;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserToken
{
    public class TotalCountAspNetUserTokenQuery : IRequest<Response<int>>
    {
        public TotalCountAspNetUserTokenQuery()
        {
        }

        public class TotalCountAspNetUserTokenHandler : IRequestHandler<TotalCountAspNetUserTokenQuery, Response<int>>
        {
            private readonly ILogger<TotalCountAspNetUserTokenHandler> _logger;
            private readonly IAspNetUserTokenRepository _repository;
            public TotalCountAspNetUserTokenHandler(ILogger<TotalCountAspNetUserTokenHandler> logger, IAspNetUserTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountAspNetUserTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}