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
    public class DeleteItemApiMethodAccessGrantCommand : IRequest<Response<bool>>
    {
        public string PermissionGroupName { get; set; }
        public string ApiMethodDefinitionKey { get; set; }

        public DeleteItemApiMethodAccessGrantCommand(string permissionGroupName, string apiMethodDefinitionKey)
        {
            this.PermissionGroupName = permissionGroupName;
            this.ApiMethodDefinitionKey = apiMethodDefinitionKey;
        }

        public class DeleteItemApiMethodAccessGrantHandler : IRequestHandler<DeleteItemApiMethodAccessGrantCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public DeleteItemApiMethodAccessGrantHandler(ILogger<DeleteItemApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemApiMethodAccessGrantCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.PermissionGroupName, request.ApiMethodDefinitionKey);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}