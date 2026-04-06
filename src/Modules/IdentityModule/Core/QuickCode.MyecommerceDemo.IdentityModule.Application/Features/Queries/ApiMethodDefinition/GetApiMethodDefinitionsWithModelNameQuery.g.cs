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
    public class GetApiMethodDefinitionsWithModelNameQuery : IRequest<Response<List<GetApiMethodDefinitionsWithModelNameResponseDto>>>
    {
        public string ApiMethodDefinitionModelName { get; set; }

        public GetApiMethodDefinitionsWithModelNameQuery(string apiMethodDefinitionModelName)
        {
            this.ApiMethodDefinitionModelName = apiMethodDefinitionModelName;
        }

        public class GetApiMethodDefinitionsWithModelNameHandler : IRequestHandler<GetApiMethodDefinitionsWithModelNameQuery, Response<List<GetApiMethodDefinitionsWithModelNameResponseDto>>>
        {
            private readonly ILogger<GetApiMethodDefinitionsWithModelNameHandler> _logger;
            private readonly IApiMethodDefinitionRepository _repository;
            public GetApiMethodDefinitionsWithModelNameHandler(ILogger<GetApiMethodDefinitionsWithModelNameHandler> logger, IApiMethodDefinitionRepository repository)
            {
                _logger = logger;
                _repository = repository;
            }

            public async Task<Response<List<GetApiMethodDefinitionsWithModelNameResponseDto>>> Handle(GetApiMethodDefinitionsWithModelNameQuery request, CancellationToken cancellationToken)
            {
                var returnValue = await _repository.GetApiMethodDefinitionsWithModelNameAsync(request.ApiMethodDefinitionModelName);
                return returnValue.ToResponse();
            }
        }
    }
}