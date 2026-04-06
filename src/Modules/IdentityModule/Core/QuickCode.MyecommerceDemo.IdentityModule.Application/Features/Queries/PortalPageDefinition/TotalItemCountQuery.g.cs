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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalPageDefinition;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalPageDefinition
{
    public class TotalCountPortalPageDefinitionQuery : IRequest<Response<int>>
    {
        public TotalCountPortalPageDefinitionQuery()
        {
        }

        public class TotalCountPortalPageDefinitionHandler : IRequestHandler<TotalCountPortalPageDefinitionQuery, Response<int>>
        {
            private readonly ILogger<TotalCountPortalPageDefinitionHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public TotalCountPortalPageDefinitionHandler(ILogger<TotalCountPortalPageDefinitionHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(TotalCountPortalPageDefinitionQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.CountAsync();
                return returnValue.ToResponse();
            }
        }
    }
}