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
    public class ListAspNetUserClaimQuery : IRequest<Response<List<AspNetUserClaimDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetUserClaimQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetUserClaimHandler : IRequestHandler<ListAspNetUserClaimQuery, Response<List<AspNetUserClaimDto>>>
        {
            private readonly ILogger<ListAspNetUserClaimHandler> _logger;
            private readonly IAspNetUserClaimRepository _repository;
            public ListAspNetUserClaimHandler(ILogger<ListAspNetUserClaimHandler> logger, IAspNetUserClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserClaimDto>>> Handle(ListAspNetUserClaimQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}