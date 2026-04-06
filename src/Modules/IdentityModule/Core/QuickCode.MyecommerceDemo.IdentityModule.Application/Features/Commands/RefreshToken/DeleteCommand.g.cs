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
    public class DeleteRefreshTokenCommand : IRequest<Response<bool>>
    {
        public RefreshTokenDto request { get; set; }

        public DeleteRefreshTokenCommand(RefreshTokenDto request)
        {
            this.request = request;
        }

        public class DeleteRefreshTokenHandler : IRequestHandler<DeleteRefreshTokenCommand, Response<bool>>
        {
            private readonly ILogger<DeleteRefreshTokenHandler> _logger;
            private readonly IRefreshTokenRepository _repository;
            public DeleteRefreshTokenHandler(ILogger<DeleteRefreshTokenHandler> logger, IRefreshTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteRefreshTokenCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.DeleteAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}