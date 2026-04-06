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
    public class DeleteApiMethodAccessGrantCommand : IRequest<Response<bool>>
    {
        public ApiMethodAccessGrantDto request { get; set; }

        public DeleteApiMethodAccessGrantCommand(ApiMethodAccessGrantDto request)
        {
            this.request = request;
        }

        public class DeleteApiMethodAccessGrantHandler : IRequestHandler<DeleteApiMethodAccessGrantCommand, Response<bool>>
        {
            private readonly ILogger<DeleteApiMethodAccessGrantHandler> _logger;
            private readonly IApiMethodAccessGrantRepository _repository;
            public DeleteApiMethodAccessGrantHandler(ILogger<DeleteApiMethodAccessGrantHandler> logger, IApiMethodAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteApiMethodAccessGrantCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}