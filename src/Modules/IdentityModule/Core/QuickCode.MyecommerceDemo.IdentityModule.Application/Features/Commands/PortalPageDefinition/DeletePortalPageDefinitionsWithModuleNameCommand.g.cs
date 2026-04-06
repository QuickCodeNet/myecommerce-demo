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
    public class DeletePortalPageDefinitionsWithModuleNameCommand : IRequest<Response<int>>
    {
        public string PortalPageDefinitionModuleName { get; set; }

        public DeletePortalPageDefinitionsWithModuleNameCommand(string portalPageDefinitionModuleName)
        {
            this.PortalPageDefinitionModuleName = portalPageDefinitionModuleName;
        }

        public class DeletePortalPageDefinitionsWithModuleNameHandler : IRequestHandler<DeletePortalPageDefinitionsWithModuleNameCommand, Response<int>>
        {
            private readonly ILogger<DeletePortalPageDefinitionsWithModuleNameHandler> _logger;
            private readonly IPortalPageDefinitionRepository _repository;
            public DeletePortalPageDefinitionsWithModuleNameHandler(ILogger<DeletePortalPageDefinitionsWithModuleNameHandler> logger, IPortalPageDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<int>> Handle(DeletePortalPageDefinitionsWithModuleNameCommand request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.DeletePortalPageDefinitionsWithModuleNameAsync(request.PortalPageDefinitionModuleName);
                return returnValue.ToResponse();
            }
        }
    }
}