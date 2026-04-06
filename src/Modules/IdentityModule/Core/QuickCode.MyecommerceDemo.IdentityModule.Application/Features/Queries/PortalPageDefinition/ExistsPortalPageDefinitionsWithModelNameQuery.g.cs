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
    public class ExistsPortalPageDefinitionsWithModelNameQuery : IRequest<Response<bool>>
    {
        public string PortalPageDefinitionModelName { get; set; }

        public ExistsPortalPageDefinitionsWithModelNameQuery(string portalPageDefinitionModelName)
        {
            this.PortalPageDefinitionModelName = portalPageDefinitionModelName;
        }

        public class ExistsPortalPageDefinitionsWithModelNameHandler : IRequestHandler<ExistsPortalPageDefinitionsWithModelNameQuery, Response<bool>>
        {
            private readonly ILogger<ExistsPortalPageDefinitionsWithModelNameHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public ExistsPortalPageDefinitionsWithModelNameHandler(ILogger<ExistsPortalPageDefinitionsWithModelNameHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(ExistsPortalPageDefinitionsWithModelNameQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ExistsPortalPageDefinitionsWithModelNameAsync(request.PortalPageDefinitionModelName);
                return returnValue.ToResponse();
            }
        }
    }
}