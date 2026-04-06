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
    public class InsertAspNetUserTokenCommand : IRequest<Response<AspNetUserTokenDto>>
    {
        public AspNetUserTokenDto request { get; set; }

        public InsertAspNetUserTokenCommand(AspNetUserTokenDto request)
        {
            this.request = request;
        }

        public class InsertAspNetUserTokenHandler : IRequestHandler<InsertAspNetUserTokenCommand, Response<AspNetUserTokenDto>>
        {
            private readonly ILogger<InsertAspNetUserTokenHandler> _logger;
            private readonly IAspNetUserTokenRepository _repository;
            public InsertAspNetUserTokenHandler(ILogger<InsertAspNetUserTokenHandler> logger, IAspNetUserTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserTokenDto>> Handle(InsertAspNetUserTokenCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}