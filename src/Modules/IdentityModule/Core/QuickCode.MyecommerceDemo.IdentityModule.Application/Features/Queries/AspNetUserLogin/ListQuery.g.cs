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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserLogin;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserLogin
{
    public class ListAspNetUserLoginQuery : IRequest<Response<List<AspNetUserLoginDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetUserLoginQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetUserLoginHandler : IRequestHandler<ListAspNetUserLoginQuery, Response<List<AspNetUserLoginDto>>>
        {
            private readonly ILogger<ListAspNetUserLoginHandler> _logger;
            private readonly IAspNetUserLoginRepository _repository;
            public ListAspNetUserLoginHandler(ILogger<ListAspNetUserLoginHandler> logger, IAspNetUserLoginRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserLoginDto>>> Handle(ListAspNetUserLoginQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}