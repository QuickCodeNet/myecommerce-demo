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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserToken;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserToken
{
    public class ListAspNetUserTokenQuery : IRequest<Response<List<AspNetUserTokenDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListAspNetUserTokenQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListAspNetUserTokenHandler : IRequestHandler<ListAspNetUserTokenQuery, Response<List<AspNetUserTokenDto>>>
        {
            private readonly ILogger<ListAspNetUserTokenHandler> _logger;
            private readonly IAspNetUserTokenRepository _repository;
            public ListAspNetUserTokenHandler(ILogger<ListAspNetUserTokenHandler> logger, IAspNetUserTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<AspNetUserTokenDto>>> Handle(ListAspNetUserTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}