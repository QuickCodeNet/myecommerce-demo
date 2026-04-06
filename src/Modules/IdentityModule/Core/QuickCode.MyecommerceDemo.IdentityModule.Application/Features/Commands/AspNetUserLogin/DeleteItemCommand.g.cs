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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUserLogin;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUserLogin
{
    public class DeleteItemAspNetUserLoginCommand : IRequest<Response<bool>>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public DeleteItemAspNetUserLoginCommand(string loginProvider, string providerKey)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        public class DeleteItemAspNetUserLoginHandler : IRequestHandler<DeleteItemAspNetUserLoginCommand, Response<bool>>
        {
            private readonly ILogger<DeleteItemAspNetUserLoginHandler> _logger;
            private readonly IAspNetUserLoginRepository _repository;
            public DeleteItemAspNetUserLoginHandler(ILogger<DeleteItemAspNetUserLoginHandler> logger, IAspNetUserLoginRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<bool>> Handle(DeleteItemAspNetUserLoginCommand request, CancellationToken cancellationToken)
            {
                var deleteItem = await _repository.GetByPkAsync(request.LoginProvider, request.ProviderKey);
                if (deleteItem.Code == 404)
                    return Response<bool>.NotFound();
                var returnValue = await _repository.DeleteAsync(deleteItem.Value);
                return returnValue.ToResponse();
            }
        }
    }
}