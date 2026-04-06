using System;
using System.Linq;
using QuickCode.MyecommerceDemo.Common.Mediator;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using QuickCode.MyecommerceDemo.Common.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Entities;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.Queries.PortalPageAccessGrant
{
    public class PortalPageAccessGrantGetItemsQuery : IRequest<Response<PortalPageAccessGrantList>>
    {
        private string PermissionGroupName { get; set; }

        public PortalPageAccessGrantGetItemsQuery(string permissionGroupName)
        {
            this.PermissionGroupName = permissionGroupName;
        }

        public class PortalPageAccessGrantGetItemsHandler : IRequestHandler<PortalPageAccessGrantGetItemsQuery, Response<PortalPageAccessGrantList>>
        {
            private readonly ILogger<PortalPageAccessGrantGetItemsHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _portalPermissionAccessRepository; 
            private readonly IPortalPageDefinitionRepository _portalPageDefinitionRepository;
            
            public PortalPageAccessGrantGetItemsHandler(ILogger<PortalPageAccessGrantGetItemsHandler> logger, 
                IPortalPageAccessGrantRepository portalPermissionAccessRepository,
                IPortalPageDefinitionRepository portalPageDefinitionRepository)
            {
                _logger = logger;
                _portalPermissionAccessRepository = portalPermissionAccessRepository;
                _portalPageDefinitionRepository= portalPageDefinitionRepository;
            }

            public async Task<Response<PortalPageAccessGrantList>> Handle(PortalPageAccessGrantGetItemsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = new Response<PortalPageAccessGrantList>
                {
                    Value = new PortalPageAccessGrantList
                    {
                        PermissionGroupName = request.PermissionGroupName,
                        PortalPageDefinitions = []
                    }
                };

                var permissionTypes = Enum.GetValues<PageActionType>();
                var permissions = (await _portalPageDefinitionRepository.ListAsync()).Value;
                var permissionAccessData = (await _portalPermissionAccessRepository.GetPortalPageAccessGrantsAsync(request.PermissionGroupName)).Value;
                foreach (var portalPermission in permissions.Where(i=>i.PageAction.Equals(PageActionType.List)).OrderBy(i=>i.Key))
                {
                    if (portalPermission.ModelName.Pluralize().Equals("AuditLogs"))
                    {
                        continue;
                    }
                    
                    var permissionName = $"{portalPermission.ModuleName}{portalPermission.ModelName}";
                    var item = new PortalPagePermissionItem()
                    {
                        PortalPagePermissionName = permissionName,
                        ModelName = portalPermission.ModelName,
                        ModuleName =  portalPermission.ModuleName,
                        PortalPagePermissionTypes = []
                    };

                    foreach (var type in permissionTypes)
                    {
                        var typeItem = new PortalPagePermissionTypeItem()
                        {
                            PortalPagePermissionType = type
                        };

                        var result = permissionAccessData!.Where(i =>
                            i.IsActive &&
                            i.PortalPageDefinitionKey.Equals($"{permissionName}{type}"));

                        typeItem.Value = result.Any();
                        item.PortalPagePermissionTypes.Add(typeItem);
                    }

                    returnValue.Value.PortalPageDefinitions.Add(item);
                }

                return returnValue;
            }
        }
    }
}
