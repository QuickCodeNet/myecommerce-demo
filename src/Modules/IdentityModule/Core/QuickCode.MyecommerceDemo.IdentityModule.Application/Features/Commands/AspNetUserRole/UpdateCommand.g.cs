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
    public class UpdateAspNetUserRoleCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public AspNetUserRoleDto request { get; set; }

        public UpdateAspNetUserRoleCommand(string userId, string roleId, AspNetUserRoleDto request)
        {
            this.request = request;
            this.UserId = userId;
            this.RoleId = roleId;
        }

        public class UpdateAspNetUserRoleHandler : IRequestHandler<UpdateAspNetUserRoleCommand, Response<bool>>
        {
            private readonly ILogger<UpdateAspNetUserRoleHandler> _logger;
            private readonly IAspNetUserRoleRepository _repository;
            public UpdateAspNetUserRoleHandler(ILogger<UpdateAspNetUserRoleHandler> logger, IAspNetUserRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateAspNetUserRoleCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.UserId, request.RoleId);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}