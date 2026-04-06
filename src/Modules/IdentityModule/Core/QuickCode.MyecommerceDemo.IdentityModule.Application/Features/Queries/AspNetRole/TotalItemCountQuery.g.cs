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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetRole;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetRole
{
    public class TotalCountAspNetRoleQuery : IRequest<Response<int>>
    {
        public TotalCountAspNetRoleQuery()
        {
        }

        public class TotalCountAspNetRoleHandler : IRequestHandler<TotalCountAspNetRoleQuery, Response<int>>
        {
            private readonly ILogger<TotalCountAspNetRoleHandler> _logger;
            private readonly IAspNetRoleRepository _repository;
            public TotalCountAspNetRoleHandler(ILogger<TotalCountAspNetRoleHandler> logger, IAspNetRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountAspNetRoleQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}