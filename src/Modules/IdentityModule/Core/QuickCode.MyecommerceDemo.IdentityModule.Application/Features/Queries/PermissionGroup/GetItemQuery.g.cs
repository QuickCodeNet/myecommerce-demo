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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PermissionGroup;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PermissionGroup
{
    public class GetItemPermissionGroupQuery : IRequest<Response<PermissionGroupDto>>
    {
        public string Name { get; set; }

        public GetItemPermissionGroupQuery(string name)
        {
            this.Name = name;
        }

        public class GetItemPermissionGroupHandler : IRequestHandler<GetItemPermissionGroupQuery, Response<PermissionGroupDto>>
        {
            private readonly ILogger<GetItemPermissionGroupHandler> _logger;
            private readonly IPermissionGroupRepository _repository;
            public GetItemPermissionGroupHandler(ILogger<GetItemPermissionGroupHandler> logger, IPermissionGroupRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PermissionGroupDto>> Handle(GetItemPermissionGroupQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Name);
                return returnValue.ToResponse();
            }
        }
    }
}