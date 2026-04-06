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
    public class DeleteItemAspNetUserTokenCommand : IRequest<Response<bool>>
    {
        public string UserId { get; set; }

        public DeleteItemAspNetUserTokenCommand(string userId)
        {
            this.UserId = userId;
        }

        public class DeleteItemAspNetUserTokenHandler : IRequestHandler<DeleteItemAspNetUserTokenCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetUserTokenHandler> _logger;
            private readonly IAspNetUserTokenRepository _repository;
            public DeleteItemAspNetUserTokenHandler(ILogger<DeleteItemAspNetUserTokenHandler> logger, IAspNetUserTokenRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetUserTokenCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.UserId);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}