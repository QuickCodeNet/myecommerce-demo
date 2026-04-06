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
    public class InsertAspNetRoleClaimCommand : IRequest<Response<AspNetRoleClaimDto>>
    {
        public AspNetRoleClaimDto request { get; set; }

        public InsertAspNetRoleClaimCommand(AspNetRoleClaimDto request)
        {
            this.request = request;
        }

        public class InsertAspNetRoleClaimHandler : IRequestHandler<InsertAspNetRoleClaimCommand, Response<AspNetRoleClaimDto>>
        {
            private readonly ILogger<InsertAspNetRoleClaimHandler> _logger;
            private readonly IAspNetRoleClaimRepository _repository;
            public InsertAspNetRoleClaimHandler(ILogger<InsertAspNetRoleClaimHandler> logger, IAspNetRoleClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRoleClaimDto>> Handle(InsertAspNetRoleClaimCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}