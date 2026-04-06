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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserLogin;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserLogin
{
    public class TotalCountAspNetUserLoginQuery : IRequest<Response<int>>
    {
        public TotalCountAspNetUserLoginQuery()
        {
        }

        public class TotalCountAspNetUserLoginHandler : IRequestHandler<TotalCountAspNetUserLoginQuery, Response<int>>
        {
            private readonly ILogger<TotalCountAspNetUserLoginHandler> _logger;
            private readonly IAspNetUserLoginRepository _repository;
            public TotalCountAspNetUserLoginHandler(ILogger<TotalCountAspNetUserLoginHandler> logger, IAspNetUserLoginRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountAspNetUserLoginQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}