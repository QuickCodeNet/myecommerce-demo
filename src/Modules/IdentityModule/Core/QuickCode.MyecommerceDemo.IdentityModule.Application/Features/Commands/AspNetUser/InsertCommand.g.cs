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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.AspNetUser;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.AspNetUser
{
    public class InsertAspNetUserCommand : IRequest<Response<AspNetUserDto>>
    {
        public AspNetUserDto request { get; set; }

        public InsertAspNetUserCommand(AspNetUserDto request)
        {
            this.request = request;
        }

        public class InsertAspNetUserHandler : IRequestHandler<InsertAspNetUserCommand, Response<AspNetUserDto>>
        {
            private readonly ILogger<InsertAspNetUserHandler> _logger;
            private readonly IAspNetUserRepository _repository;
            public InsertAspNetUserHandler(ILogger<InsertAspNetUserHandler> logger, IAspNetUserRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<AspNetUserDto>> Handle(InsertAspNetUserCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}