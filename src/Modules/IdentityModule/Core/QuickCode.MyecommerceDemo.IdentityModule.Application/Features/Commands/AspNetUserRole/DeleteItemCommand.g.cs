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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserRole;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserRole
{
    public class DeleteItemAspNetUserRoleCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public DeleteItemAspNetUserRoleCommand(string userId, string roleId)
        {
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public class DeleteItemAspNetUserRoleHandler : IRequestHandler<DeleteItemAspNetUserRoleCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetUserRoleHandler> _logger;
            private readonly IAspNetUserRoleRepository _repository;
            public DeleteItemAspNetUserRoleHandler(ILogger<DeleteItemAspNetUserRoleHandler> logger, IAspNetUserRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetUserRoleCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.UserId, request.RoleId);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}