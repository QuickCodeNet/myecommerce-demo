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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodAccessGrant
{
    public class TotalCountApiMethodAccessGrantQuery : IRequest<Response<int>>
    {
        public TotalCountApiMethodAccessGrantQuery()
        {
        }

        public class TotalCountApiMethodAccessGrantHandler : IRequestHandler<TotalCountApiMethodAccessGrantQuery, Response<int>>
        {
            private readonly ILogger<TotalCountApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public TotalCountApiMethodAccessGrantHandler(ILogger<TotalCountApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountApiMethodAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}