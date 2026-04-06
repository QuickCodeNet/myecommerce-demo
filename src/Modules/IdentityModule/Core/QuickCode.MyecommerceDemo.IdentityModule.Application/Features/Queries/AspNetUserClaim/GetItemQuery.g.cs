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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserClaim;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserClaim
{
    public class GetItemAspNetUserClaimQuery : IRequest<Response<AspNetUserClaimDto>>
    {
        public int Id { get; set; }

        public GetItemAspNetUserClaimQuery(int id)
        {
            this.Id = id;
        }

        public class GetItemAspNetUserClaimHandler : IRequestHandler<GetItemAspNetUserClaimQuery, Response<AspNetUserClaimDto>>
        {
            private readonly ILogger<GetItemAspNetUserClaimHandler> _logger;
            private readonly IAspNetUserClaimRepository _repository;
            public GetItemAspNetUserClaimHandler(ILogger<GetItemAspNetUserClaimHandler> logger, IAspNetUserClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserClaimDto>> Handle(GetItemAspNetUserClaimQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.Id);
                return returnValue.ToResponse();
            }
        }
    }
}