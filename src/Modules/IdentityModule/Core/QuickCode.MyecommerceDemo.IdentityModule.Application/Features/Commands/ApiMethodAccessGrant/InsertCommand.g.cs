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
    public class InsertApiMethodAccessGrantCommand : IRequest<Response<ApiMethodAccessGrantDto>>
    {
        public ApiMethodAccessGrantDto request { get; set; }

        public InsertApiMethodAccessGrantCommand(ApiMethodAccessGrantDto request)
        {
            this.request = request;
        }

        public class InsertApiMethodAccessGrantHandler : IRequestHandler<InsertApiMethodAccessGrantCommand, Response<ApiMethodAccessGrantDto>>
        {
            private readonly ILogger<InsertApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public InsertApiMethodAccessGrantHandler(ILogger<InsertApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodAccessGrantDto>> Handle(InsertApiMethodAccessGrantCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}