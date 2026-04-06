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
    public class DeleteItemPortalPageAccessGrantCommand : IRequest<Response<bool>>
    {
        public string PermissionGroupName { get; set; }
        public string PortalPageDefinitionKey { get; set; }
        public PageActionType PageAction { get; set; }

        public DeleteItemPortalPageAccessGrantCommand(string permissionGroupName, string portalPageDefinitionKey, PageActionType pageAction)
        {
            this.PermissionGroupName = permissionGroupName;
            this.PortalPageDefinitionKey = portalPageDefinitionKey;
            this.PageAction = pageAction;
        }

        public class DeleteItemPortalPageAccessGrantHandler : IRequestHandler<DeleteItemPortalPageAccessGrantCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemPortalPageAccessGrantHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _repository;
            public DeleteItemPortalPageAccessGrantHandler(ILogger<DeleteItemPortalPageAccessGrantHandler> logger, IPortalPageAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemPortalPageAccessGrantCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.PermissionGroupName, request.PortalPageDefinitionKey, request.PageAction);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}