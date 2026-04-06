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
    public class GetItemAspNetUserLoginQuery : IRequest<Response<AspNetUserLoginDto>>
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }

        public GetItemAspNetUserLoginQuery(string loginProvider, string providerKey)
        {
            this.LoginProvider = loginProvider;
            this.ProviderKey = providerKey;
        }

        public class GetItemAspNetUserLoginHandler : IRequestHandler<GetItemAspNetUserLoginQuery, Response<AspNetUserLoginDto>>
        {
            private readonly ILogger<GetItemAspNetUserLoginHandler> _logger;
            private readonly IAspNetUserLoginRepository _repository;
            public GetItemAspNetUserLoginHandler(ILogger<GetItemAspNetUserLoginHandler> logger, IAspNetUserLoginRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserLoginDto>> Handle(GetItemAspNetUserLoginQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetByPkAsync(request.LoginProvider, request.ProviderKey);
                return returnValue.ToResponse();
            }
        }
    }
}