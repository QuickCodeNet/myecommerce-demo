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
    public class UpdateRefreshTokenCommand : IRequest<Response<bool>>
    {
        public int Id { get; set; }
        public RefreshTokenDto request { get; set; }

        public UpdateRefreshTokenCommand(int id, RefreshTokenDto request)
        {
            this.request = request;
            this.Id = id;
        }

        public class UpdateRefreshTokenHandler : IRequestHandler<UpdateRefreshTokenCommand, Response<bool>>
        {
            private readonly ILogger<UpdateRefreshTokenHandler> _logger;
            private readonly IRefreshTokenRepository _repository;
            public UpdateRefreshTokenHandler(ILogger<UpdateRefreshTokenHandler> logger, IRefreshTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
            {
                var updateItem = await _repository.GetByPkAsync(request.Id);
                if (updateItem.Code == 404)
                    return Response<bool>.NotFound();
                var model = request.request;
                var returnValue = await _repository.UpdateAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}