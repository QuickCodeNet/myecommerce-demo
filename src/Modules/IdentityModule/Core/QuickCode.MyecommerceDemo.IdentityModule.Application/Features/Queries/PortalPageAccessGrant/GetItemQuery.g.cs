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
    public class GetItemPortalPageAccessGrantQuery : IRequest<Response<PortalPageAccessGrantDto>>
    {
        public string PermissionGroupName { get; set; }
        public string PortalPageDefinitionKey { get; set; }
        public PageActionType PageAction { get; set; }

        public GetItemPortalPageAccessGrantQuery(string permissionGroupName, string portalPageDefinitionKey, PageActionType pageAction)
        {
            this.PermissionGroupName = permissionGroupName;
            this.PortalPageDefinitionKey = portalPageDefinitionKey;
            this.PageAction = pageAction;
        }

        public class GetItemPortalPageAccessGrantHandler : IRequestHandler<GetItemPortalPageAccessGrantQuery, Response<PortalPageAccessGrantDto>>
        {
            private readonly ILogger<GetItemPortalPageAccessGrantHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _repository;
            public GetItemPortalPageAccessGrantHandler(ILogger<GetItemPortalPageAccessGrantHandler> logger, IPortalPageAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPageAccessGrantDto>> Handle(GetItemPortalPageAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.PermissionGroupName, request.PortalPageDefinitionKey, request.PageAction);
                return returnValue.ToResponse();
            }
        }
    }
}