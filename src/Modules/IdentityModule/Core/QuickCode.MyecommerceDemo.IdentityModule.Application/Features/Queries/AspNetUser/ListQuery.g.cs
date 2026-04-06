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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUser;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUser
{
    public class ListAspNetUserQuery : IRequest<Response<List<AspNetUserDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetUserQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetUserHandler : IRequestHandler<ListAspNetUserQuery, Response<List<AspNetUserDto>>>
        {
            private readonly ILogger<ListAspNetUserHandler> _logger;
            private readonly IAspNetUserRepository _repository;
            public ListAspNetUserHandler(ILogger<ListAspNetUserHandler> logger, IAspNetUserRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserDto>>> Handle(ListAspNetUserQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}