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
    public class DeleteAspNetUserRoleCommand : IRequest<Response<bool>>
    {
        public AspNetUserRoleDto request { get; set; }

        public DeleteAspNetUserRoleCommand(AspNetUserRoleDto request)
        {
            this.request = request;
        }

        public class DeleteAspNetUserRoleHandler : IRequestHandler<DeleteAspNetUserRoleCommand, Response<bool>>
        {
            private readonly ILogger<DeleteAspNetUserRoleHandler> _logger;
            private readonly IAspNetUserRoleRepository _repository;
            public DeleteAspNetUserRoleHandler(ILogger<DeleteAspNetUserRoleHandler> logger, IAspNetUserRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteAspNetUserRoleCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}