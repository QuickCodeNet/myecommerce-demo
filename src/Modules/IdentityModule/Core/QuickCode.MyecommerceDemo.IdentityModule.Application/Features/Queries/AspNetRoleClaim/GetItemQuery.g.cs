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
    public class GetItemAspNetRoleClaimQuery : IRequest<Response<AspNetRoleClaimDto>>
    {
        public int Id { get; set; }

        public GetItemAspNetRoleClaimQuery(int id)
        {
            this.Id = id;
        }

        public class GetItemAspNetRoleClaimHandler : IRequestHandler<GetItemAspNetRoleClaimQuery, Response<AspNetRoleClaimDto>>
        {
            private readonly ILogger<GetItemAspNetRoleClaimHandler> _logger;
            private readonly IAspNetRoleClaimRepository _repository;
            public GetItemAspNetRoleClaimHandler(ILogger<GetItemAspNetRoleClaimHandler> logger, IAspNetRoleClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetRoleClaimDto>> Handle(GetItemAspNetRoleClaimQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Id);
                return returnValue.ToResponse();
            }
        }
    }
}