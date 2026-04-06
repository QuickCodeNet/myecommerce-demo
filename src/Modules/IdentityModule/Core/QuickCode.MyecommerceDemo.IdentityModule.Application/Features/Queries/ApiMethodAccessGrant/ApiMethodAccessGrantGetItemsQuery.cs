using System.Linq;
using QuickCode.MyecommerceDemo.Common.Mediator;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Humanizer;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Models;
using QuickCode.MyecommerceDemo.IdentityModule.Application.Interfaces.Repositories;
using QuickCode.MyecommerceDemo.Common.Helpers;
using QuickCode.MyecommerceDemo.Common.Models;

namespace QuickCode.MyecommerceDemo.IdentityModule.Api.Application.Features.Queries.ApiMethodAccessGrant
{
    public class ApiMethodAccessGrantGetItemsQuery : IRequest<Response<ApiModulePermissions>>
    {
        public string PermissionGroupName { get; set; }

        public ApiMethodAccessGrantGetItemsQuery(string permissionGroupName)
        {
            this.PermissionGroupName = permissionGroupName;
        }

        public class AuthorizationsApiGroupsGetItemsHandler : IRequestHandler<ApiMethodAccessGrantGetItemsQuery, Response<ApiModulePermissions>>
        {
            private readonly ILogger<AuthorizationsApiGroupsGetItemsHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _apiMethodAccessGrantRepository;
            private readonly IApiMethodDefinitionRepository _apiMethodDefinitionRepository;
            
            public AuthorizationsApiGroupsGetItemsHandler(ILogger<AuthorizationsApiGroupsGetItemsHandler> logger, 
                IApiMethodAccessGrantRepository apiMethodAccessGrantRepository,
                IApiMethodDefinitionRepository apiPermissionsRepository)
            {
                _logger = logger;
                _apiMethodAccessGrantRepository = apiMethodAccessGrantRepository;
                _apiMethodDefinitionRepository = apiPermissionsRepository;
            }

            public async Task<Response<ApiModulePermissions>> Handle(ApiMethodAccessGrantGetItemsQuery request, CancellationToken cancellationToken)
            {
                var returnValue = new Response<ApiModulePermissions>
                {
                    Value = new ApiModulePermissions
                    {
                        PermissionGroupName = request.PermissionGroupName,
                        ApiModulePermissionList = []
                    }
                };
                
                var authorizationsApis = (await _apiMethodDefinitionRepository.ListAsync()).Value.OrderBy(i => i.UrlPath)
                    .ThenBy(i => i.ControllerName);

                var authorizationApiGroupData =
                    (await _apiMethodAccessGrantRepository.GetApiMethodAccessGrantNamesAsync(
                        request.PermissionGroupName)).Value;
                foreach (var authorizationApi in authorizationsApis.OrderBy(i => i.ControllerName)
                             .ThenBy(i => i.HttpMethod))
                {

                    if (authorizationApi.ModelName.Pluralize().Equals("AuditLogs"))
                    {
                        continue;
                    }
                    
                    var isExists = authorizationApiGroupData.Exists(i =>
                        i.PermissionGroupName.Equals(request.PermissionGroupName) && i.ApiMethodDefinitionKey.Equals(authorizationApi.Key));

                    var item = new ApiMethodDefinitionItem
                    {
                        Key = authorizationApi.Key,
                        ControllerName = authorizationApi.ControllerName,
                        HttpMethod = authorizationApi.HttpMethod,
                        UrlPath = authorizationApi.UrlPath,
                        Value = isExists
                    };

                    var controllerItems = new Dictionary<string, List<ApiMethodDefinitionItem>>();
                    var itemList = new List<ApiMethodDefinitionItem>();
                    var moduleName = item.UrlPath.Split('/')[2].KebabCaseToPascal();

                    if (item.ControllerName.Equals("AuthenticationsController"))
                    {
                        continue;
                    }

                    var controllerName = item.ControllerName[0..^"Controller".Length].PascalToKebabCase().KebabCaseToPascal();
                    returnValue.Value.ApiModulePermissionList.TryAdd(moduleName, controllerItems);
                    controllerItems = returnValue.Value.ApiModulePermissionList[moduleName];
                    controllerItems.TryAdd(controllerName, itemList);
                    returnValue.Value.ApiModulePermissionList[moduleName][controllerName].Add(item);
                }

                return returnValue;
            }
        }
    }
}
