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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetRoleClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetRoleClaim
{
    public class DeleteAspNetRoleClaimCommand : IRequest<Response<bool>>
    {
        public AspNetRoleClaimDto request { get; set; }

        public DeleteAspNetRoleClaimCommand(AspNetRoleClaimDto request)
        {
            this.request = request;
        }

        public class DeleteAspNetRoleClaimHandler : IRequestHandler<DeleteAspNetRoleClaimCommand, Response<bool>>
        {
            private readonly ILogger<DeleteAspNetRoleClaimHandler> _logger;
            private readonly IAspNetRoleClaimRepository _repository;
            public DeleteAspNetRoleClaimHandler(ILogger<DeleteAspNetRoleClaimHandler> logger, IAspNetRoleClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteAspNetRoleClaimCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}