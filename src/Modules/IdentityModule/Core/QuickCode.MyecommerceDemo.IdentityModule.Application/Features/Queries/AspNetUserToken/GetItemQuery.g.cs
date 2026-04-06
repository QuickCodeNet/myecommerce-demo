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
    public class GetItemAspNetUserTokenQuery : IRequest<Response<AspNetUserTokenDto>>
    {
        public string UserId { get; set; }

        public GetItemAspNetUserTokenQuery(string userId)
        {
            this.UserId = userId;
        }

        public class GetItemAspNetUserTokenHandler : IRequestHandler<GetItemAspNetUserTokenQuery, Response<AspNetUserTokenDto>>
        {
            private readonly ILogger<GetItemAspNetUserTokenHandler> _logger;
            private readonly IAspNetUserTokenRepository _repository;
            public GetItemAspNetUserTokenHandler(ILogger<GetItemAspNetUserTokenHandler> logger, IAspNetUserTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserTokenDto>> Handle(GetItemAspNetUserTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.UserId);
                return returnValue.ToResponse();
            }
        }
    }
}