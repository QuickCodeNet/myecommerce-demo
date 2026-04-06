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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.PortalPageAccessGrant;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.PortalPageAccessGrant
{
    public class InsertPortalPageAccessGrantCommand : IRequest<Response<PortalPageAccessGrantDto>>
    {
        public PortalPageAccessGrantDto request { get; set; }

        public InsertPortalPageAccessGrantCommand(PortalPageAccessGrantDto request)
        {
            this.request = request;
        }

        public class InsertPortalPageAccessGrantHandler : IRequestHandler<InsertPortalPageAccessGrantCommand, Response<PortalPageAccessGrantDto>>
        {
            private readonly ILogger<InsertPortalPageAccessGrantHandler> _logger;
            private readonly IPortalPageAccessGrantRepository _repository;
            public InsertPortalPageAccessGrantHandler(ILogger<InsertPortalPageAccessGrantHandler> logger, IPortalPageAccessGrantRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<PortalPageAccessGrantDto>> Handle(InsertPortalPageAccessGrantCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}