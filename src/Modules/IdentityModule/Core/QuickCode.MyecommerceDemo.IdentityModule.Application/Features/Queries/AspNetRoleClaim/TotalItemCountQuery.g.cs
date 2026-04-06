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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetRoleClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetRoleClaim
{
    public class TotalCountAspNetRoleClaimQuery : IRequest<Response<int>>
    {
        public TotalCountAspNetRoleClaimQuery()
        {
        }

        public class TotalCountAspNetRoleClaimHandler : IRequestHandler<TotalCountAspNetRoleClaimQuery, Response<int>>
        {
            private readonly ILogger<TotalCountAspNetRoleClaimHandler> _logger;
            private readonly IAspNetRoleClaimRepository _repository;
            public TotalCountAspNetRoleClaimHandler(ILogger<TotalCountAspNetRoleClaimHandler> logger, IAspNetRoleClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountAspNetRoleClaimQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}