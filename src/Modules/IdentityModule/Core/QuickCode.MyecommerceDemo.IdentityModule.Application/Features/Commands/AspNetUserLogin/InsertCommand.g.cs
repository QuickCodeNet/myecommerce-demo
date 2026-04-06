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
    public class InsertAspNetUserLoginCommand : IRequest<Response<AspNetUserLoginDto>>
    {
        public AspNetUserLoginDto request { get; set; }

        public InsertAspNetUserLoginCommand(AspNetUserLoginDto request)
        {
            this.request = request;
        }

        public class InsertAspNetUserLoginHandler : IRequestHandler<InsertAspNetUserLoginCommand, Response<AspNetUserLoginDto>>
        {
            private readonly ILogger<InsertAspNetUserLoginHandler> _logger;
            private readonly IAspNetUserLoginRepository _repository;
            public InsertAspNetUserLoginHandler(ILogger<InsertAspNetUserLoginHandler> logger, IAspNetUserLoginRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserLoginDto>> Handle(InsertAspNetUserLoginCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}