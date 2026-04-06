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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetRole;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetRole
{
    public class ListAspNetRoleQuery : IRequest<Response<List<AspNetRoleDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetRoleQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetRoleHandler : IRequestHandler<ListAspNetRoleQuery, Response<List<AspNetRoleDto>>>
        {
            private readonly ILogger<ListAspNetRoleHandler> _logger;
            private readonly IAspNetRoleRepository _repository;
            public ListAspNetRoleHandler(ILogger<ListAspNetRoleHandler> logger, IAspNetRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetRoleDto>>> Handle(ListAspNetRoleQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}