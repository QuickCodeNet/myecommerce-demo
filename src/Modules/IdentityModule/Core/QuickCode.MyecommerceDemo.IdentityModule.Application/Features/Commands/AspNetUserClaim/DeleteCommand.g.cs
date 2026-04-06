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
    public class DeleteAspNetUserClaimCommand : IRequest<Response<bool>>
    {
        public AspNetUserClaimDto request { get; set; }

        public DeleteAspNetUserClaimCommand(AspNetUserClaimDto request)
        {
            this.request = request;
        }

        public class DeleteAspNetUserClaimHandler : IRequestHandler<DeleteAspNetUserClaimCommand, Response<bool>>
        {
            private readonly ILogger<DeleteAspNetUserClaimHandler> _logger;
            private readonly IAspNetUserClaimRepository _repository;
            public DeleteAspNetUserClaimHandler(ILogger<DeleteAspNetUserClaimHandler> logger, IAspNetUserClaimRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteAspNetUserClaimCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}