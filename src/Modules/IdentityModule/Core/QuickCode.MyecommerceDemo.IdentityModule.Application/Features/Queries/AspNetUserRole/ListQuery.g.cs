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
    public class ListAspNetUserRoleQuery : IRequest<Response<List<AspNetUserRoleDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetUserRoleQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetUserRoleHandler : IRequestHandler<ListAspNetUserRoleQuery, Response<List<AspNetUserRoleDto>>>
        {
            private readonly ILogger<ListAspNetUserRoleHandler> _logger;
            private readonly IAspNetUserRoleRepository _repository;
            public ListAspNetUserRoleHandler(ILogger<ListAspNetUserRoleHandler> logger, IAspNetUserRoleRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserRoleDto>>> Handle(ListAspNetUserRoleQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}