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
    public class GetPortalPageAccessGrantQuery : IRequest<Response<List<GetPortalPageAccessGrantResponseDto>>>
    {
        public string PortalPageAccessGrantPermissionGroupName { get; set; }
        public string PortalPageAccessGrantPortalPageDefinitionKey { get; set; }
        public PageActionType PortalPageAccessGrantPageAction { get; set; }

        public GetPortalPageAccessGrantQuery(string portalPageAccessGrantPermissionGroupName, string portalPageAccessGrantPortalPageDefinitionKey, PageActionType portalPageAccessGrantPageAction)
        {
            this.PortalPageAccessGrantPermissionGroupName = portalPageAccessGrantPermissionGroupName;
            this.PortalPageAccessGrantPortalPageDefinitionKey = portalPageAccessGrantPortalPageDefinitionKey;
            this.PortalPageAccessGrantPageAction = portalPageAccessGrantPageAction;
        }

        public class GetPortalPageAccessGrantHandler : IRequestHandler<GetPortalPageAccessGrantQuery, Response<List<GetPortalPageAccessGrantResponseDto>>>
        {
            private readonly ILogger<GetPortalPageAccessGrantHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _repository;
            public GetPortalPageAccessGrantHandler(ILogger<GetPortalPageAccessGrantHandler> logger, IPortalPageAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetPortalPageAccessGrantResponseDto>>> Handle(GetPortalPageAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetPortalPageAccessGrantAsync(request.PortalPageAccessGrantPermissionGroupName, request.PortalPageAccessGrantPortalPageDefinitionKey, request.PortalPageAccessGrantPageAction);
                return returnValue.ToResponse();
            }
        }
    }
}