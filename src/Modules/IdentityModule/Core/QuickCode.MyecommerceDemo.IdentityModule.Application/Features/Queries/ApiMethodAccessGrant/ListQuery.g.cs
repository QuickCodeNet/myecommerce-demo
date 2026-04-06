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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodAccessGrant
{
    public class ListApiMethodAccessGrantQuery : IRequest<Response<List<ApiMethodAccessGrantDto>>>
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }

        public ListApiMethodAccessGrantQuery(int? pageNumber, int? pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }

        public class ListApiMethodAccessGrantHandler : IRequestHandler<ListApiMethodAccessGrantQuery, Response<List<ApiMethodAccessGrantDto>>>
        {
            private readonly ILogger<ListApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public ListApiMethodAccessGrantHandler(ILogger<ListApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<ApiMethodAccessGrantDto>>> Handle(ListApiMethodAccessGrantQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.ListAsync(request.PageNumber, request.PageSize);
                return returnValue.ToResponse();
            }
        }
    }
}