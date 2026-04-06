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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalPageAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalPageAccessGrant
{
    public class TotalCountPortalPageAccessGrantQuery : IRequest<Response<int>>
    {
        public TotalCountPortalPageAccessGrantQuery()
        {
        }

        public class TotalCountPortalPageAccessGrantHandler : IRequestHandler<TotalCountPortalPageAccessGrantQuery, Response<int>>
        {
            private readonly ILogger<TotalCountPortalPageAccessGrantHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _repository;
            public TotalCountPortalPageAccessGrantHandler(ILogger<TotalCountPortalPageAccessGrantHandler> logger, IPortalPageAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountPortalPageAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}