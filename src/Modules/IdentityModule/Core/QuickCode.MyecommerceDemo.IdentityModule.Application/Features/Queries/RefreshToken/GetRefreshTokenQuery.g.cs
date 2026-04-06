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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.RefreshToken;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.RefreshToken
{
    public class GetRefreshTokenQuery : IRequest<Response<GetRefreshTokenResponseDto>>
    {
        public string RefreshTokenToken { get; set; }

        public GetRefreshTokenQuery(string refreshTokenToken)
        {
            this.RefreshTokenToken = refreshTokenToken;
        }

        public class GetRefreshTokenHandler : IRequestHandler<GetRefreshTokenQuery, Response<GetRefreshTokenResponseDto>>
        {
            private readonly ILogger<GetRefreshTokenHandler> _logger;
            private readonly IRefreshTokenRepository _repository;
            public GetRefreshTokenHandler(ILogger<GetRefreshTokenHandler> logger, IRefreshTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<GetRefreshTokenResponseDto>> Handle(GetRefreshTokenQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetRefreshTokenAsync(request.RefreshTokenToken);
                return returnValue.ToResponse();
            }
        }
    }
}