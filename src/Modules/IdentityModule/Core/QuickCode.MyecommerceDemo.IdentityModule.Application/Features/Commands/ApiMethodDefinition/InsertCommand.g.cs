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
using QuickCode.MyecommerceDemo.IdentityModule.Application.Dtos.ApiMethodDefinition;
using QuickCode.MyecommerceDemo.IdentityModule.Domain.Enums;

namespace QuickCode.MyecommerceDemo.IdentityModule.Application.Features.ApiMethodDefinition
{
    public class InsertApiMethodDefinitionCommand : IRequest<Response<ApiMethodDefinitionDto>>
    {
        public ApiMethodDefinitionDto request { get; set; }

        public InsertApiMethodDefinitionCommand(ApiMethodDefinitionDto request)
        {
            this.request = request;
        }

        public class InsertApiMethodDefinitionHandler : IRequestHandler<InsertApiMethodDefinitionCommand, Response<ApiMethodDefinitionDto>>
        {
            private readonly ILogger<InsertApiMethodDefinitionHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public InsertApiMethodDefinitionHandler(ILogger<InsertApiMethodDefinitionHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<ApiMethodDefinitionDto>> Handle(InsertApiMethodDefinitionCommand request, CancellationToken cancellationToken)
            {
                var model = request.request;
                var returnValue = await _repository.InsertAsync(model);
                return returnValue.ToResponse();
            }
        }
    }
}