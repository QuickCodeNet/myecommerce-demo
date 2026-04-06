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
    public class GetItemApiMethodAccessGrantQuery : IRequest<Response<ApiMethodAccessGrantDto>>
    {
        public string PermissionGroupName { get; set; }
        public string ApiMethodDefinitionKey { get; set; }

        public GetItemApiMethodAccessGrantQuery(string permissionGroupName, string apiMethodDefinitionKey)
        {
            this.PermissionGroupName = permissionGroupName;
            this.ApiMethodDefinitionKey = apiMethodDefinitionKey;
        }

        public class GetItemApiMethodAccessGrantHandler : IRequestHandler<GetItemApiMethodAccessGrantQuery, Response<ApiMethodAccessGrantDto>>
        {
            private readonly ILogger<GetItemApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public GetItemApiMethodAccessGrantHandler(ILogger<GetItemApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodAccessGrantDto>> Handle(GetItemApiMethodAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.PermissionGroupName, request.ApiMethodDefinitionKey);
                return returnValue.ToResponse();
            }
        }
    }
}