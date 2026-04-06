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
    public class GetPortalPageDefinitionsWithModelNameQuery : IRequest<Response<List<GetPortalPageDefinitionsWithModelNameResponseDto>>>
    {
        public string PortalPageDefinitionModelName { get; set; }

        public GetPortalPageDefinitionsWithModelNameQuery(string portalPageDefinitionModelName)
        {
            this.PortalPageDefinitionModelName = portalPageDefinitionModelName;
        }

        public class GetPortalPageDefinitionsWithModelNameHandler : IRequestHandler<GetPortalPageDefinitionsWithModelNameQuery, Response<List<GetPortalPageDefinitionsWithModelNameResponseDto>>>
        {
            private readonly ILogger<GetPortalPageDefinitionsWithModelNameHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public GetPortalPageDefinitionsWithModelNameHandler(ILogger<GetPortalPageDefinitionsWithModelNameHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetPortalPageDefinitionsWithModelNameResponseDto>>> Handle(GetPortalPageDefinitionsWithModelNameQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetPortalPageDefinitionsWithModelNameAsync(request.PortalPageDefinitionModelName);
                return returnValue.ToResponse();
            }
        }
    }
}